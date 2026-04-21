using Bookly.API.Models.Curtida;
using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurtidaController : ControllerBase
{
    private readonly ICurtidaAplicacao _curtidaAplicacao;

    public CurtidaController(ICurtidaAplicacao curtidaAplicacao)
    {
        _curtidaAplicacao = curtidaAplicacao;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var curtidas = await _curtidaAplicacao.ListarAsync();
        var response = curtidas.Select(c => new CurtidaResponse
        {
            Id = c.Id,
            UsuarioId = c.UsuarioId,
            AvaliacaoId = c.AvaliacaoId,
            DataCriacao = c.DataCriacao
        });
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var curtida = await _curtidaAplicacao.ObterPorIdAsync(id);
            var response = new CurtidaResponse
            {
                Id = curtida.Id,
                UsuarioId = curtida.UsuarioId,
                AvaliacaoId = curtida.AvaliacaoId,
                DataCriacao = curtida.DataCriacao
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarCurtidaRequest request)
    {
        try
        {
            var curtida = new Curtida
            {
                UsuarioId = request.UsuarioId,
                AvaliacaoId = request.AvaliacaoId
            };
            var id = await _curtidaAplicacao.CriarAsync(curtida);
            return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
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
            await _curtidaAplicacao.DeletarAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}
