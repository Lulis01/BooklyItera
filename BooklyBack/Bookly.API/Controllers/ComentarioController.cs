using Bookly.API.Models.Comentario;
using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComentarioController : ControllerBase
{
    private readonly IComentarioAplicacao _comentarioAplicacao;

    public ComentarioController(IComentarioAplicacao comentarioAplicacao)
    {
        _comentarioAplicacao = comentarioAplicacao;
    }

    [HttpGet("Listar")]
    public async Task<IActionResult> Listar()
    {
        var comentarios = await _comentarioAplicacao.ListarAsync();
        var response = comentarios.Select(c => new ComentarioResponse
        {
            Id = c.Id,
            UsuarioId = c.UsuarioId,
            AvaliacaoId = c.AvaliacaoId,
            Texto = c.Texto,
            DataCriacao = c.DataCriacao
        });
        return Ok(response);
    }

    [HttpGet("Obter/{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var comentario = await _comentarioAplicacao.ObterPorIdAsync(id);
            var response = new ComentarioResponse
            {
                Id = comentario.Id,
                UsuarioId = comentario.UsuarioId,
                AvaliacaoId = comentario.AvaliacaoId,
                Texto = comentario.Texto,
                DataCriacao = comentario.DataCriacao
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpPost("Criar")]
    public async Task<IActionResult> Criar([FromBody] CriarComentarioRequest request)
    {
        try
        {
            var comentario = new Comentario
            {
                UsuarioId = request.UsuarioId,
                AvaliacaoId = request.AvaliacaoId,
                Texto = request.Texto
            };
            await _comentarioAplicacao.CriarAsync(comentario);
            var response = new ComentarioResponse
            {
                Id = comentario.Id,
                UsuarioId = comentario.UsuarioId,
                AvaliacaoId = comentario.AvaliacaoId,
                Texto = comentario.Texto,
                DataCriacao = comentario.DataCriacao
            };
            return CreatedAtAction(nameof(ObterPorId), new { id = comentario.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("Atualizar/{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarComentarioRequest request)
    {
        try
        {
            var comentario = new Comentario
            {
                Id = id,
                Texto = request.Texto
            };
            await _comentarioAplicacao.AtualizarAsync(comentario);
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
            await _comentarioAplicacao.DeletarAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}
