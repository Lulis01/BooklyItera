using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Bookly.Dominio.Interfaces;

namespace Bookly.Aplicacao;

public class UsuarioAplicacao : IUsuarioAplicacao
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    public async Task<int> CriarAsync(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario), "Usuário não pode ser vazio");

        if (string.IsNullOrEmpty(usuario.Nome))
            throw new Exception("Nome não pode ser vazio");

        if (string.IsNullOrEmpty(usuario.Email))
            throw new Exception("Email não pode ser vazio");

        if (string.IsNullOrEmpty(usuario.SenhaHash))
            throw new Exception("Senha não pode ser vazia");

        return await _usuarioRepositorio.CriarAsync(usuario);
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        var usuarioExistente = await _usuarioRepositorio.ObterPorIdAsync(usuario.Id);
        if (usuarioExistente == null)
            throw new Exception("Usuário não encontrado");

        if (string.IsNullOrEmpty(usuario.Nome))
            throw new Exception("Nome não pode ser vazio");

        if (string.IsNullOrEmpty(usuario.Email))
            throw new Exception("Email não pode ser vazio");

        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;

        await _usuarioRepositorio.AtualizarAsync(usuarioExistente);
    }

    public async Task DeletarAsync(Guid id)
    {
        var usuarioExistente = await _usuarioRepositorio.ObterPorIdAsync(id);
        if (usuarioExistente == null)
            throw new Exception("Usuário não encontrado");

        await _usuarioRepositorio.DeletarAsync(id);
    }

    public async Task<Usuario> ObterPorIdAsync(Guid id)
    {
        var usuario = await _usuarioRepositorio.ObterPorIdAsync(id);
        if (usuario == null)
            throw new Exception("Usuário não encontrado");

        return usuario;
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        var usuario = await _usuarioRepositorio.ObterPorEmailAsync(email);
        if (usuario == null)
            throw new Exception("Email não encontrado");

        return usuario;
    }

    public async Task<IEnumerable<Usuario>> ListarAsync()
    {
        return await _usuarioRepositorio.ListarAsync();
    }
}
