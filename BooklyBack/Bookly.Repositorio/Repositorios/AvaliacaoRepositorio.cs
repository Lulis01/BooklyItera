using Bookly.Dominio.Entidades;
using Bookly.Dominio.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Bookly.Repositorio.Repositorios;

public class AvaliacaoRepositorio : IAvaliacaoRepositorio
{
    private readonly string _connectionString;

    public AvaliacaoRepositorio(string connectionString)
    {
        _connectionString = connectionString;
    }

    private SqlConnection CriarConexao() => new SqlConnection(_connectionString);

    public async Task<int> CriarAsync(Avaliacao avaliacao)
    {
        if (avaliacao.Id == Guid.Empty)
            avaliacao.Id = Guid.NewGuid();

        using var conexao = CriarConexao();

        return await conexao.ExecuteAsync(
            "sp_CriarAvaliacao",
            new
            {
                avaliacao.Id,
                avaliacao.UsuarioId,
                avaliacao.LivroId,
                avaliacao.Texto,
                avaliacao.Nota,
                avaliacao.DataCriacao
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Avaliacao> ObterPorIdAsync(Guid id)
    {
        using var conexao = CriarConexao();

        return await conexao.QueryFirstOrDefaultAsync<Avaliacao>(
            "sp_ObterAvaliacaoPorId",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Avaliacao>> ListarAsync()
    {
        using var conexao = CriarConexao();

        return await conexao.QueryAsync<Avaliacao>(
            "sp_ListarAvaliacoes",
            commandType: CommandType.StoredProcedure);
    }

    public async Task AtualizarAsync(Avaliacao avaliacao)
    {
        using var conexao = CriarConexao();

        await conexao.ExecuteAsync(
            "sp_AtualizarAvaliacao",
            new
            {
                avaliacao.Id,
                avaliacao.Texto,
                avaliacao.Nota
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeletarAsync(Guid id)
    {
        using var conexao = CriarConexao();

        await conexao.ExecuteAsync(
            "sp_DeletarAvaliacao",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }
}
