using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Bookly.Dominio.Interfaces;

namespace Bookly.Aplicacao;

public class CurtidaAplicacao : ICurtidaAplicacao
{
    private readonly ICurtidaRepositorio _curtidaRepositorio;

    public CurtidaAplicacao(ICurtidaRepositorio curtidaRepositorio)
    {
        _curtidaRepositorio = curtidaRepositorio;
    }

    public async Task<int> CriarAsync(Curtida curtida)
    {
        if (curtida == null)
            throw new ArgumentNullException(nameof(curtida), "Curtida não pode ser vazia");

        if (curtida.UsuarioId == Guid.Empty)
            throw new Exception("Usuário da curtida não pode ser vazio");

        if (curtida.AvaliacaoId == Guid.Empty)
            throw new Exception("Avaliação da curtida não pode ser vazia");

        return await _curtidaRepositorio.CriarAsync(curtida);
    }

    public async Task DeletarAsync(Guid id)
    {
        var curtidaExistente = await _curtidaRepositorio.ObterPorIdAsync(id);
        if (curtidaExistente == null)
            throw new Exception("Curtida não encontrada");

        await _curtidaRepositorio.DeletarAsync(id);
    }

    public async Task<Curtida> ObterPorIdAsync(Guid id)
    {
        var curtida = await _curtidaRepositorio.ObterPorIdAsync(id);
        if (curtida == null)
            throw new Exception("Curtida não encontrada");

        return curtida;
    }

    public async Task<IEnumerable<Curtida>> ListarAsync()
    {
        return await _curtidaRepositorio.ListarAsync();
    }
}
