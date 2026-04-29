using Bookly.Aplicacao.Interfaces;
using Bookly.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly IRecomendacaoAplicacao _recomendacaoAplicacao;

    public ChatbotController(IRecomendacaoAplicacao recomendacaoAplicacao)
    {
        _recomendacaoAplicacao = recomendacaoAplicacao;
    }

    [HttpPost("Mensagem")]
    public async Task<IActionResult> Mensagem([FromBody] ChatbotRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Mensagem))
            return BadRequest(new { mensagem = "A mensagem não pode ser vazia." });

        try
        {
            var resposta = await _recomendacaoAplicacao.EnviarMensagemChatbotAsync(request.Mensagem);
            return Ok(resposta);
        }
        catch (HttpRequestException ex)
        {
            return new JsonResult(
                new { mensagem = "Erro ao comunicar com a IA.", detalhe = ex.Message }
            );
        }
    }
}
