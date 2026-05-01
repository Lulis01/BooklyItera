using Bookly.Dominio.Entidades;
using Bookly.Dominio.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Bookly.Repositorio.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly string _connectionString;

    public UsuarioRepositorio(string connectionString)
    {
        _connectionString = connectionString;
    }

    private SqlConnection CriarConexao() => new SqlConnection(_connectionString);

    public async Task<int> CriarAsync(Usuario usuario)
    {
        if (usuario.Id == Guid.Empty)
            usuario.Id = Guid.NewGuid();

        using var conexao = CriarConexao();

        return await conexao.ExecuteAsync(
            "sp_CriarUsuario",
            new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                usuario.SenhaHash,
                usuario.DataCriacao
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        using var conexao = CriarConexao();

        return await conexao.QueryFirstOrDefaultAsync<Usuario>(
            "sp_ObterUsuarioPorId",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        using var conexao = CriarConexao();

        return await conexao.QueryFirstOrDefaultAsync<Usuario>(
            "sp_ObterUsuarioPorEmail",
            new { Email = email },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Usuario>> ListarAsync()
    {
        using var conexao = CriarConexao();

        return await conexao.QueryAsync<Usuario>(
            "sp_ListarUsuarios",
            commandType: CommandType.StoredProcedure);
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        using var conexao = CriarConexao();

        await conexao.ExecuteAsync(
            "sp_AtualizarUsuario",
            new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                usuario.SenhaHash
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeletarAsync(Guid id)
    {
        using var conexao = CriarConexao();

        await conexao.ExecuteAsync(
            "sp_DeletarUsuario",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }
}
