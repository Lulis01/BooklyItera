using Bookly.Dominio.Entidades;
using Bookly.Dominio.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Bookly.Repositorio.Repositorios;

public class CurtidaRepositorio : ICurtidaRepositorio
{
    private readonly string _connectionString;

    public CurtidaRepositorio(string connectionString)
    {
        _connectionString = connectionString;
    }

    private SqlConnection CriarConexao() => new SqlConnection(_connectionString);

    public async Task<int> CriarAsync(Curtida curtida)
    {
        if (curtida.Id == Guid.Empty)
            curtida.Id = Guid.NewGuid();

        using var conexao = CriarConexao();

        return await conexao.ExecuteAsync(
            "sp_CriarCurtida",
            new
            {
                curtida.Id,
                curtida.UsuarioId,
                curtida.AvaliacaoId,
                curtida.DataCriacao
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Curtida> ObterPorIdAsync(Guid id)
    {
        using var conexao = CriarConexao();

        return await conexao.QueryFirstOrDefaultAsync<Curtida>(
            "sp_ObterCurtidaPorId",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Curtida>> ListarAsync()
    {
        using var conexao = CriarConexao();

        return await conexao.QueryAsync<Curtida>(
            "sp_ListarCurtidas",
            commandType: CommandType.StoredProcedure);
    }

    /// <summary>
    /// Curtida é imutável após criação. Não possui campos editáveis.
    /// Para desfazer uma curtida, use DeletarAsync e recrie com CriarAsync.
    /// </summary>
    public Task AtualizarAsync(Curtida curtida)
    {
        throw new NotSupportedException(
            "Curtidas não podem ser atualizadas. Delete e recrie se necessário.");
    }

    public async Task DeletarAsync(Guid id)
    {
        using var conexao = CriarConexao();

        await conexao.ExecuteAsync(
            "sp_DeletarCurtida",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }
}
