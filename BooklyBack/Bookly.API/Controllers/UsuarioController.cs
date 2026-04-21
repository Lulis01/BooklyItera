using Bookly.API.Models.Usuario;
using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioAplicacao _usuarioAplicacao;

    public UsuarioController(IUsuarioAplicacao usuarioAplicacao)
    {
        _usuarioAplicacao = usuarioAplicacao;
    }

    [HttpGet]
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

    [HttpGet("{id:guid}")]
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

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest request)
    {
        try
        {
            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = request.SenhaHash
            };
            var id = await _usuarioAplicacao.CriarAsync(usuario);
            return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
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

    [HttpDelete("{id:guid}")]
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
