using Bookly.API.Models.Avaliacao;
using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvaliacaoController : ControllerBase
{
    private readonly IAvaliacaoAplicacao _avaliacaoAplicacao;

    public AvaliacaoController(IAvaliacaoAplicacao avaliacaoAplicacao)
    {
        _avaliacaoAplicacao = avaliacaoAplicacao;
    }

    [HttpGet("Listar")]
    public async Task<IActionResult> Listar()
    {
        var avaliacoes = await _avaliacaoAplicacao.ListarAsync();
        var response = avaliacoes.Select(a => new AvaliacaoResponse
        {
            Id = a.Id,
            UsuarioId = a.UsuarioId,
            LivroId = a.LivroId,
            Texto = a.Texto,
            Nota = a.Nota,
            DataCriacao = a.DataCriacao
        });
        return Ok(response);
    }

    [HttpGet("Obter/{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var avaliacao = await _avaliacaoAplicacao.ObterPorIdAsync(id);
            var response = new AvaliacaoResponse
            {
                Id = avaliacao.Id,
                UsuarioId = avaliacao.UsuarioId,
                LivroId = avaliacao.LivroId,
                Texto = avaliacao.Texto,
                Nota = avaliacao.Nota,
                DataCriacao = avaliacao.DataCriacao
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpPost("Criar")]
    public async Task<IActionResult> Criar([FromBody] CriarAvaliacaoRequest request)
    {
        try
        {
            var avaliacao = new Avaliacao
            {
                UsuarioId = request.UsuarioId,
                LivroId = request.LivroId,
                Texto = request.Texto,
                Nota = request.Nota
            };
            await _avaliacaoAplicacao.CriarAsync(avaliacao);
            return CreatedAtAction(nameof(ObterPorId), new { id = avaliacao.Id }, new { id = avaliacao.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("Atualizar/{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarAvaliacaoRequest request)
    {
        try
        {
            var avaliacao = new Avaliacao
            {
                Id = id,
                Texto = request.Texto,
                Nota = request.Nota
            };
            await _avaliacaoAplicacao.AtualizarAsync(avaliacao);
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
            await _avaliacaoAplicacao.DeletarAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}
