import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TopBar } from '../../Componentes/TopBar/TopBar';
import estilo from './Avaliar.module.css';
import { useAuth } from '../../Context/AuthContext';
import LivroAPI from '../../Services/livroAPI';
import AvaliacaoAPI from '../../Services/avaliacaoAPI';

function Avaliar() {
    const navegar = useNavigate();
    const { user } = useAuth();
    
    const [termoBusca, setTermoBusca] = useState('');
    const [listaLivros, setListaLivros] = useState([]);
    const [livroEscolhido, setLivroEscolhido] = useState(null);
    const [notaDada, setNotaDada] = useState(5);
    const [comentario, setComentario] = useState('');

    async function procurarLivros(evento) {
        evento.preventDefault();
        try {
            const resposta = await LivroAPI.importarAsync(termoBusca);
            setListaLivros(resposta.importados || []);
        } catch (erro) {
            alert("Ops! Deu erro ao buscar os livros.");
        }
    }

    async function enviarAvaliacao(evento) {
        evento.preventDefault();
        try {
            const usuarioId = user?.id || user?.Id;
            const livroId = livroEscolhido?.id || livroEscolhido?.Id;

            if (!livroId) {
                alert("Por favor, selecione um livro primeiro!");
                return;
            }

            await AvaliacaoAPI.criarAsync(usuarioId, livroId, comentario, notaDada);

            alert("Boa! Sua avaliação foi enviada com sucesso.");
            navegar('/home');
        } catch (erro) {
            console.error(erro);
            alert("Erro ao enviar. Verifique se você escreveu o comentário.");
        }
    }

    return (
        <TopBar>
            <div className={estilo.pagina_conteudo}>
                <h2 className={estilo.titulo}>Avaliar Livro</h2>
                <p className={estilo.subtitulo}>Busque um livro e deixe sua nota.</p>

                <form onSubmit={procurarLivros} className={estilo.busca_container}>
                    <input 
                        type="text" 
                        placeholder="Digite o nome do livro..." 
                        value={termoBusca} 
                        onChange={e => setTermoBusca(e.target.value)} 
                        className={estilo.input_busca}
                    />
                    <button type="submit" className={estilo.botao_busca}>
                        Buscar
                    </button>
                </form>

                {livroEscolhido === null && listaLivros.length > 0 && (
                    <div className={estilo.lista_livros}>
                        <div className={estilo.lista_header}>
                            Clique no livro para selecionar:
                        </div>
                        {listaLivros.map(livro => (
                            <div 
                                key={livro.id || livro.Id} 
                                onClick={() => setLivroEscolhido(livro)} 
                                className={estilo.livro_item}
                            >
                                <strong>{livro.titulo}</strong> <br/>
                                <small className={estilo.autor_texto}>{livro.autor}</small>
                            </div>
                        ))}
                    </div>
                )}

                {livroEscolhido !== null && (
                    <div className={estilo.form_container}>
                        <p className={estilo.info_livro}>Avaliar: <strong>{livroEscolhido.titulo}</strong></p>
                        <button onClick={() => setLivroEscolhido(null)} className={estilo.btn_mudar}>
                            Mudar de livro
                        </button>
                        
                        <form onSubmit={enviarAvaliacao} className={estilo.form_main}>
                            <div className={estilo.campo}>
                                <label className={estilo.label}>Sua nota: </label>
                                <div className={estilo.estrelas_container}>
                                    {[1, 2, 3, 4, 5].map(num => (
                                        <span 
                                            key={num} 
                                            className={num <= notaDada ? estilo.estrela_ativa : estilo.estrela_inativa}
                                            onClick={() => setNotaDada(num)}
                                        >
                                            ★
                                        </span>
                                    ))}
                                </div>
                            </div>
                            <div className={estilo.campo}>
                                <label className={estilo.label}>Seu comentário: </label>
                                <textarea 
                                    placeholder="O que você achou desta leitura?" 
                                    value={comentario} 
                                    onChange={e => setComentario(e.target.value)}
                                    className={estilo.textarea}
                                    maxLength={500}
                                    required
                                />
                                <div className={estilo.contador}>
                                    {comentario.length} / 500 caracteres
                                </div>
                            </div>
                            <button type="submit" className={estilo.botao_postar}>
                                Publicar Avaliação
                            </button>
                        </form>
                    </div>
                )}
            </div>
        </TopBar>
    );
}

export default Avaliar;
