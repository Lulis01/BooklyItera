using Bookly.API.Models.Livro;
using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivroController : ControllerBase
{
    private readonly ILivroAplicacao _livroAplicacao;

    public LivroController(ILivroAplicacao livroAplicacao)
    {
        _livroAplicacao = livroAplicacao;
    }

    [HttpGet]
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

    [HttpGet("{id:guid}")]
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

    [HttpGet("buscar")]
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

    [HttpPost]
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
            var id = await _livroAplicacao.CriarAsync(livro);
            return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
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

    [HttpDelete("{id:guid}")]
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
