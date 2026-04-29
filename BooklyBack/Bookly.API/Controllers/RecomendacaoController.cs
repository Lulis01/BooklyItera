using Bookly.Aplicacao.Interfaces;
using Bookly.Services.DTOs;
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
        if (request == null || request.Avaliacoes == null || !request.Avaliacoes.Any())
        {
            return BadRequest(new { mensagem = "É necessário enviar ao menos uma avaliação para gerar recomendações." });
        }

        try
        {
            var recomendacoes = await _recomendacaoAplicacao.GerarRecomendacoesAsync(request.Avaliacoes);
            return Ok(recomendacoes);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = "Erro ao gerar recomendações pela IA.", detalhe = ex.Message });
        }
    }
}

public class RecomendacaoRequest
{
    public IEnumerable<LivroAvaliadoDto> Avaliacoes { get; set; } = new List<LivroAvaliadoDto>();
}
