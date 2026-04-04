using Bookly.Dominio.Entidades;
using Bookly.Dominio.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Bookly.Repositorio.Repositorios;

public class ComentarioRepositorio : IComentarioRepositorio
{
    private readonly string _connectionString;

    public ComentarioRepositorio(string connectionString)
    {
        _connectionString = connectionString;
    }

    private SqlConnection CriarConexao() => new SqlConnection(_connectionString);

    public async Task<int> CriarAsync(Comentario comentario)
    {
        if (comentario.Id == Guid.Empty)
            comentario.Id = Guid.NewGuid();

        using var conexao = CriarConexao();

        return await conexao.ExecuteAsync(
            "sp_CriarComentario",
            new
            {
                comentario.Id,
                comentario.UsuarioId,
                comentario.AvaliacaoId,
                comentario.Texto,
                comentario.DataCriacao
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Comentario> ObterPorIdAsync(Guid id)
    {
        using var conexao = CriarConexao();

        return await conexao.QueryFirstOrDefaultAsync<Comentario>(
            "sp_ObterComentarioPorId",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Comentario>> ListarAsync()
    {
        using var conexao = CriarConexao();

        return await conexao.QueryAsync<Comentario>(
            "sp_ListarComentarios",
            commandType: CommandType.StoredProcedure);
    }

    public async Task AtualizarAsync(Comentario comentario)
    {
        using var conexao = CriarConexao();

        await conexao.ExecuteAsync(
            "sp_AtualizarComentario",
            new
            {
                comentario.Id,
                comentario.Texto
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeletarAsync(Guid id)
    {
        using var conexao = CriarConexao();

        await conexao.ExecuteAsync(
            "sp_DeletarComentario",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }
}
