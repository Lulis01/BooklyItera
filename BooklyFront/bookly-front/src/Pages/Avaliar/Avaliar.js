import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TopBar } from '../../Componentes/TopBar/TopBar';
import style from './Avaliar.module.css';
import { useAuth } from '../../Context/AuthContext';
import LivroAPI from '../../Services/livroAPI';
import AvaliacaoAPI from '../../Services/avaliacaoAPI';

function Avaliar() {
    const navigate = useNavigate();
    const { user } = useAuth();
    
    const [busca, setBusca] = useState('');
    const [livros, setLivros] = useState([]);
    const [livroSelecionado, setLivroSelecionado] = useState(null);
    const [nota, setNota] = useState(5);
    const [texto, setTexto] = useState('');

    
    async function buscarLivros(e) {
        e.preventDefault();
        try {
            const response = await LivroAPI.importarAsync(busca);
            setLivros(response.importados || []);
        } catch (error) {
            alert("Erro ao buscar livros.");
        }
    }


    async function salvarAvaliacao(e) {
        e.preventDefault();
        try {
            const usuarioId = user?.id || user?.Id;
            const livroId = livroSelecionado?.id || livroSelecionado?.Id;

            if (!livroId) {
                alert("Selecione um livro primeiro!");
                return;
            }

            await AvaliacaoAPI.criarAsync(usuarioId, livroId, texto, nota);

            alert("Boa! Avaliação enviada.");
            navigate('/home');
        } catch (error) {
            console.error(error);
            alert("Erro ao enviar. Verifique se preencheu tudo.");
        }
    }

    return (
        <TopBar>
            <div className={style.pagina_conteudo}>
                <h2 className={style.titulo}>Avaliar Livro</h2>
                <p className={style.subtitulo}>Busque um livro e deixe sua nota.</p>

                
                <form onSubmit={buscarLivros} className={style.busca_container}>
                    <input 
                        type="text" 
                        placeholder="Nome do livro..." 
                        value={busca} 
                        onChange={e => setBusca(e.target.value)} 
                        className={style.input_busca}
                    />
                    <button type="submit" className={style.botao_busca}>
                        Buscar
                    </button>
                </form>

                
                {!livroSelecionado && livros.length > 0 && (
                    <div className={style.lista_livros}>
                        <div className={style.lista_header}>
                            Clique no livro para selecionar:
                        </div>
                        {livros.map(l => (
                            <div 
                                key={l.id || l.Id} 
                                onClick={() => setLivroSelecionado(l)} 
                                className={style.livro_item}
                            >
                                <strong>{l.titulo}</strong> <br/>
                                <small className={style.autor_texto}>{l.autor}</small>
                            </div>
                        ))}
                    </div>
                )}

                
                {livroSelecionado && (
                    <div className={style.form_container}>
                        <p className={style.info_livro}>Avaliar: <strong>{livroSelecionado.titulo}</strong></p>
                        <button onClick={() => setLivroSelecionado(null)} className={style.btn_mudar}>
                            Mudar de livro
                        </button>
                        
                        <form onSubmit={salvarAvaliacao} className={style.form_main}>
                            <div className={style.campo}>
                                <label className={style.label}>Sua nota: </label>
                                <div className={style.estrelas_container}>
                                    {[1, 2, 3, 4, 5].map(num => (
                                        <span 
                                            key={num} 
                                            className={num <= nota ? style.estrela_ativa : style.estrela_inativa}
                                            onClick={() => setNota(num)}
                                        >
                                            ★
                                        </span>
                                    ))}
                                </div>
                            </div>
                            <div className={style.campo}>
                                <label className={style.label}>Seu comentário: </label>
                                <textarea 
                                    placeholder="O que você achou desta leitura?" 
                                    value={texto} 
                                    onChange={e => setTexto(e.target.value)}
                                    className={style.textarea}
                                    required
                                />
                            </div>
                            <button type="submit" className={style.botao_postar}>
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
