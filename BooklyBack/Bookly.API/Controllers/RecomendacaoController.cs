using Bookly.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecomendacaoController : ControllerBase
{
    private readonly IRecomendacaoAplicacao _recomendacaoAplicacao;

    public RecomendacaoController(IRecomendacaoAplicacao recomendacaoAplicacao)
    {
        _recomendacaoAplicacao = recomendacaoAplicacao;
    }

    [HttpPost("Recomendar")]
    public async Task<IActionResult> Recomendar([FromBody] RecomendacaoRequest request)
    {
        if (request == null || request.UsuarioId == Guid.Empty)
        {
            return BadRequest(new { mensagem = "É necessário informar o usuário para gerar recomendações." });
        }

        try
        {
            var recomendacoes = await _recomendacaoAplicacao.GerarRecomendacoesPorUsuarioAsync(request.UsuarioId);
            return Ok(recomendacoes);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = "Erro ao gerar recomendações pela IA.", detalhe = ex.Message });
        }
    }
}

public class RecomendacaoRequest
{
    public Guid UsuarioId { get; set; }
}
