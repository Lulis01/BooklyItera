using Bookly.Dominio.Entidades;
using Bookly.Dominio.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Bookly.Repositorio.Repositorios;

public class LivroRepositorio : ILivroRepositorio
{
    private readonly string _connectionString;

    public LivroRepositorio(string connectionString)
    {
        _connectionString = connectionString;
    }

    private SqlConnection CriarConexao() => new SqlConnection(_connectionString);

    public async Task<int> CriarAsync(Livro livro)
    {
        if (livro.Id == Guid.Empty)
            livro.Id = Guid.NewGuid();

        using var conexao = CriarConexao();

        return await conexao.ExecuteAsync(
            "sp_CriarLivro",
            new
            {
                livro.Id,
                livro.Titulo,
                livro.Autor,
                livro.ISBN,
                livro.AnoPublicacao,
                livro.Genero,
                livro.DataCriacao
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Livro> ObterPorIdAsync(Guid id)
    {
        using var conexao = CriarConexao();

        return await conexao.QueryFirstOrDefaultAsync<Livro>(
            "sp_ObterLivroPorId",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Livro>> ListarAsync()
    {
        using var conexao = CriarConexao();

        return await conexao.QueryAsync<Livro>(
            "sp_ListarLivros",
            commandType: CommandType.StoredProcedure);
    }

    public async Task AtualizarAsync(Livro livro)
    {
        using var conexao = CriarConexao();

        await conexao.ExecuteAsync(
            "sp_AtualizarLivro",
            new
            {
                livro.Id,
                livro.Titulo,
                livro.Autor,
                livro.ISBN,
                livro.AnoPublicacao,
                livro.Genero
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeletarAsync(Guid id)
    {
        using var conexao = CriarConexao();

        await conexao.ExecuteAsync(
            "sp_DeletarLivro",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }
}
