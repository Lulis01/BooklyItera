using Bookly.API.Models.Livro;
using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Bookly.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivroController : ControllerBase
{
    private readonly ILivroAplicacao _livroAplicacao;
    private readonly IOpenLibraryService _openLibraryService;

    public LivroController(ILivroAplicacao livroAplicacao, IOpenLibraryService openLibraryService)
    {
        _livroAplicacao = livroAplicacao;
        _openLibraryService = openLibraryService;
    }

    [HttpGet("Listar")]
    public async Task<IActionResult> Listar()
    {
        var livros = await _livroAplicacao.ListarAsync();
        var response = livros.Select(l => new LivroResponse
        {
            Id = l.Id,
            Titulo = l.Titulo,
            Autor = l.Autor,
            ISBN = l.ISBN,
            AnoPublicacao = l.AnoPublicacao,
            Genero = l.Genero,
            DataCriacao = l.DataCriacao
        });
        return Ok(response);
    }

    [HttpGet("Obter/{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var livro = await _livroAplicacao.ObterPorIdAsync(id);
            var response = new LivroResponse
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                ISBN = livro.ISBN,
                AnoPublicacao = livro.AnoPublicacao,
                Genero = livro.Genero,
                DataCriacao = livro.DataCriacao
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpGet("Buscar")]
    public async Task<IActionResult> BuscarExternos([FromQuery] string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return BadRequest(new { mensagem = "O título para busca não pode ser vazio." });

        var livros = await _livroAplicacao.BuscarLivrosExternosAsync(titulo);
        var response = livros.Select(l => new LivroResponse
        {
            Id = l.Id,
            Titulo = l.Titulo,
            Autor = l.Autor,
            ISBN = l.ISBN,
            AnoPublicacao = l.AnoPublicacao,
            Genero = l.Genero,
            DataCriacao = l.DataCriacao
        });
        return Ok(response);
    }

    [HttpPost("Importar")]
    public async Task<IActionResult> ImportarDaApiExterna([FromQuery] string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return BadRequest(new { mensagem = "Informe um título para buscar na API externa." });

        try
        {
            var livrosExternos = await _openLibraryService.BuscarLivrosPorTituloAsync(titulo);

            if (!livrosExternos.Any())
                return NotFound(new { mensagem = "Nenhum livro encontrado na API externa com esse título." });

            var importados = new List<object>();
            var erros = new List<string>();

            foreach (var livro in livrosExternos)
            {
                try
                {
                    // Garante um Id novo local independente do temporário da API
                    livro.Id = Guid.Empty;
                    await _livroAplicacao.CriarAsync(livro);
                    importados.Add(new { livro.Id, livro.Titulo, livro.Autor });
                }
                catch (Exception ex)
                {
                    // ISBN duplicado ou outro erro — pula para o próximo
                    erros.Add($"{livro.Titulo}: {ex.Message}");
                }
            }

            return Ok(new
            {
                mensagem = $"{importados.Count} livro(s) importado(s) com sucesso.",
                importados,
                erros
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPost("Criar")]
    public async Task<IActionResult> Criar([FromBody] CriarLivroRequest request)
    {
        try
        {
            var livro = new Livro
            {
                Titulo = request.Titulo,
                Autor = request.Autor,
                ISBN = request.ISBN,
                AnoPublicacao = request.AnoPublicacao,
                Genero = request.Genero
            };
            await _livroAplicacao.CriarAsync(livro);
            return CreatedAtAction(nameof(ObterPorId), new { id = livro.Id }, new { id = livro.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("Atualizar/{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarLivroRequest request)
    {
        try
        {
            var livro = new Livro
            {
                Id = id,
                Titulo = request.Titulo,
                Autor = request.Autor,
                ISBN = request.ISBN,
                AnoPublicacao = request.AnoPublicacao,
                Genero = request.Genero
            };
            await _livroAplicacao.AtualizarAsync(livro);
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
            await _livroAplicacao.DeletarAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}
