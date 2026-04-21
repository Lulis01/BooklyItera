using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Bookly.Dominio.Interfaces;

namespace Bookly.Aplicacao;

public class AvaliacaoAplicacao : IAvaliacaoAplicacao
{
    private readonly IAvaliacaoRepositorio _avaliacaoRepositorio;

    public AvaliacaoAplicacao(IAvaliacaoRepositorio avaliacaoRepositorio)
    {
        _avaliacaoRepositorio = avaliacaoRepositorio;
    }

    public async Task<int> CriarAsync(Avaliacao avaliacao)
    {
        if (avaliacao == null)
            throw new ArgumentNullException(nameof(avaliacao), "Avaliação não pode ser vazia");

        if (avaliacao.UsuarioId == Guid.Empty)
            throw new Exception("Usuário da avaliação não pode ser vazio");

        if (avaliacao.LivroId == Guid.Empty)
            throw new Exception("Livro da avaliação não pode ser vazio");

        if (avaliacao.Nota < 1 || avaliacao.Nota > 5)
            throw new Exception("A nota deve ser entre 1 e 5.");

        return await _avaliacaoRepositorio.CriarAsync(avaliacao);
    }

    public async Task AtualizarAsync(Avaliacao avaliacao)
    {
        var avaliacaoExistente = await _avaliacaoRepositorio.ObterPorIdAsync(avaliacao.Id);
        if (avaliacaoExistente == null)
            throw new Exception("Avaliação não encontrada");

        if (avaliacao.Nota < 1 || avaliacao.Nota > 5)
            throw new Exception("A nota deve ser entre 1 e 5.");

        avaliacaoExistente.Nota = avaliacao.Nota;
        avaliacaoExistente.Texto = avaliacao.Texto;

        await _avaliacaoRepositorio.AtualizarAsync(avaliacaoExistente);
    }

    public async Task DeletarAsync(Guid id)
    {
        var avaliacaoExistente = await _avaliacaoRepositorio.ObterPorIdAsync(id);
        if (avaliacaoExistente == null)
            throw new Exception("Avaliação não encontrada");

        await _avaliacaoRepositorio.DeletarAsync(id);
    }

    public async Task<Avaliacao> ObterPorIdAsync(Guid id)
    {
        var avaliacao = await _avaliacaoRepositorio.ObterPorIdAsync(id);
        if (avaliacao == null)
            throw new Exception("Avaliação não encontrada");

        return avaliacao;
    }

    public async Task<IEnumerable<Avaliacao>> ListarAsync()
    {
        return await _avaliacaoRepositorio.ListarAsync();
    }
}
