import React, { useEffect, useState } from 'react';
import { TopBar } from '../../Componentes/TopBar/TopBar';
import estilo from './MinhasAvaliacoes.module.css';
import { useAuth } from '../../Context/AuthContext';
import AvaliacaoAPI from '../../Services/avaliacaoAPI';
import LivroAPI from '../../Services/livroAPI';

function MinhasAvaliacoes() {
    const { user } = useAuth();
    const [minhasNotas, setMinhasNotas] = useState([]);
    const [estaCarregando, setEstaCarregando] = useState(true);

    useEffect(() => {
        async function carregarDados() {
            try {
                setEstaCarregando(true);
                
                const avaliacoes = await AvaliacaoAPI.listarAsync();
                const livros = await LivroAPI.listarAsync();

                const meuId = user?.id || user?.Id;
                const minhasAvaliacoes = avaliacoes.filter(a => a.usuarioId === meuId);

                const listaFinal = minhasAvaliacoes.map(nota => {
                    const livro = livros.find(l => l.id === nota.livroId);
                    return {
                        ...nota,
                        tituloLivro: livro ? livro.titulo : "Livro Desconhecido",
                        autorLivro: livro ? livro.autor : "Desconhecido"
                    };
                });

                setMinhasNotas(listaFinal);
            } catch (erro) {
                console.error(erro);
            } finally {
                setEstaCarregando(false);
            }
        }

        if (user) carregarDados();
    }, [user]);

    return (
        <TopBar>
            <div className={estilo.pagina_container}>
                <div className={estilo.cabecalho}>
                    <h2>Minhas Avaliações</h2>
                    <p>Veja seus livros avaliados.</p>
                </div>

                {estaCarregando ? (
                    <div className={estilo.aviso}>Carregando...</div>
                ) : minhasNotas.length > 0 ? (
                    <div className={estilo.lista_notas}>
                        {minhasNotas.map(item => (
                            <div key={item.id} className={estilo.card_nota}>
                                <div className={estilo.info_livro}>
                                    <h3>{item.tituloLivro}</h3>
                                    <span>{item.autorLivro}</span>
                                </div>
                                
                                <div className={estilo.corpo_nota}>
                                    <div className={estilo.estrelas}>
                                        {"★".repeat(item.nota)}{"☆".repeat(5 - item.nota)}
                                    </div>
                                    <p className={estilo.comentario}>{item.texto}</p>
                                    <small className={estilo.data}>{new Date(item.dataCriacao).toLocaleDateString()}</small>
                                </div>
                            </div>
                        ))}
                    </div>
                ) : (
                    <div className={estilo.vazio}>
                        <p>Nenhuma avaliação feita ainda.</p>
                    </div>
                )}
            </div>
        </TopBar>
    );
}

export default MinhasAvaliacoes;
