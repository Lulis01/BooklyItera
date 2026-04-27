using Bookly.API.Models.Usuario;
using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Bookly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioAplicacao _usuarioAplicacao;
    private readonly ITokenService _tokenService;

    public UsuarioController(IUsuarioAplicacao usuarioAplicacao, ITokenService tokenService)
    {
        _usuarioAplicacao = usuarioAplicacao;
        _tokenService = tokenService;
    }

    [HttpGet("Listar")]
    public async Task<IActionResult> Listar()
    {
        var usuarios = await _usuarioAplicacao.ListarAsync();
        var response = usuarios.Select(u => new UsuarioResponse
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            DataCriacao = u.DataCriacao
        });
        return Ok(response);
    }

    [HttpGet("Obter/{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var usuario = await _usuarioAplicacao.ObterPorIdAsync(id);
            var response = new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataCriacao = usuario.DataCriacao
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpPost("Criar")]
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest request)
    {
        try
        {
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.SenhaHash);
            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = senhaHash
            };
            await _usuarioAplicacao.CriarAsync(usuario);
            return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, new { id = usuario.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginRequest request)
    {
        try
        {
            var usuario = await _usuarioAplicacao.ObterPorEmailAsync(request.Email);
            
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            {
                return BadRequest(new { mensagem = "Email ou senha inválidos" });
            }

            var accessToken = _tokenService.GerarAccessToken(usuario.Id.ToString());
            var refreshToken = _tokenService.GerarRefreshToken(usuario.Id.ToString());

            var producao = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
            
            Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = producao,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                MaxAge = TimeSpan.FromHours(8)
            });

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = producao,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                MaxAge = TimeSpan.FromDays(7)
            });

            return Ok(new 
            { 
                Id = usuario.Id, 
                Nome = usuario.Nome, 
                Email = usuario.Email, 
                AccessToken = accessToken, 
                RefreshToken = refreshToken 
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { mensagem = "Refresh token não encontrado." });

            var usuarioId = _tokenService.ValidarRefreshToken(refreshToken);

            if (usuarioId == null)
                return BadRequest(new { mensagem = "Refresh token inválido." });

            var usuario = await _usuarioAplicacao.ObterPorIdAsync(Guid.Parse(usuarioId));

            if (usuario == null)
                return BadRequest(new { mensagem = "Usuário não encontrado." });

            var novoAccessToken = _tokenService.GerarAccessToken(usuarioId);
            var novoRefreshToken = _tokenService.GerarRefreshToken(usuarioId);

            var producao = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

            Response.Cookies.Append("accessToken", novoAccessToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = producao,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                MaxAge = TimeSpan.FromHours(8)
            });

            Response.Cookies.Append("refreshToken", novoRefreshToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = producao,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                MaxAge = TimeSpan.FromDays(7)
            });

            return Ok(new
            {
                AccessToken = novoAccessToken
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("Atualizar/{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarUsuarioRequest request)
    {
        try
        {
            var usuario = new Usuario
            {
                Id = id,
                Nome = request.Nome,
                Email = request.Email
            };
            await _usuarioAplicacao.AtualizarAsync(usuario);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("Deletar/{id:guid}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            await _usuarioAplicacao.DeletarAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}
