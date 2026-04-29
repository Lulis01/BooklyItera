import React, { useEffect, useState } from 'react';
import { TopBar } from '../../Componentes/TopBar/TopBar';
import ReviewCard from '../../Componentes/ReviewCard/ReviewCard';
import estilo from './Home.module.css';
import AvaliacaoAPI from '../../Services/avaliacaoAPI';
import UsuarioAPI from '../../Services/usuarioAPI';
import LivroAPI from '../../Services/livroAPI';
import CurtidaAPI from '../../Services/curtidaAPI';
import ComentarioAPI from '../../Services/comentarioAPI';

function Home() {
    const [dadosDoFeed, setDadosDoFeed] = useState([]);
    const [estaCarregando, setEstaCarregando] = useState(true);
    const [mensagemErro, setMensagemErro] = useState(null);

    useEffect(() => {
        const carregarFeed = async () => {
            try {
                setEstaCarregando(true);
                const [avaliacoes, usuarios, livros, curtidas, comentarios] = await Promise.all([
                    AvaliacaoAPI.listarAsync(),
                    UsuarioAPI.listarAsync(),
                    LivroAPI.listarAsync(),
                    CurtidaAPI.listarAsync(),
                    ComentarioAPI.listarAsync()
                ]);

                const mapaUsuarios = usuarios.reduce((acumulador, user) => ({ ...acumulador, [user.id]: user }), {});
                const mapaLivros = livros.reduce((acumulador, book) => ({ ...acumulador, [book.id]: book }), {});

                const feedCompleto = avaliacoes.map(review => {
                    const curtidasDaAvaliacao = curtidas.filter(c => c.avaliacaoId === review.id);
                    const comentariosDaAvaliacao = comentarios.filter(c => c.avaliacaoId === review.id);

                    return {
                        review,
                        user: mapaUsuarios[review.usuarioId],
                        book: mapaLivros[review.livroId],
                        likes: curtidasDaAvaliacao,
                        comments: comentariosDaAvaliacao,
                        userMap: mapaUsuarios
                    };
                });

                feedCompleto.sort((a, b) => new Date(b.review.dataCriacao) - new Date(a.review.dataCriacao));
                setDadosDoFeed(feedCompleto);
            } catch (erro) {
                console.error(erro);
                setMensagemErro("Não foi possível carregar o feed agora.");
            } finally {
                setEstaCarregando(false);
            }
        };

        carregarFeed();
    }, []);

    return (
        <div className={estilo.conteudo}>
            <TopBar>
                <div className={estilo.pagina_conteudo}>
                    <div className={estilo.feed_header}>
                        <h2>Feed de Atividade</h2>
                        <p>Veja o que a comunidade está lendo e avaliando.</p>
                    </div>

                    {estaCarregando === true ? (
                        <div className={estilo.status_message}>Carregando o feed...</div>
                    ) : mensagemErro !== null ? (
                        <div className={estilo.error_message}>{mensagemErro}</div>
                    ) : dadosDoFeed.length > 0 ? (
                        <div className={estilo.feed_container}>
                            {dadosDoFeed.map(item => (
                                <ReviewCard 
                                    key={item.review.id}
                                    review={item.review}
                                    user={item.user}
                                    book={item.book}
                                    likes={item.likes}
                                    comments={item.comments}
                                    userMap={item.userMap}
                                />
                            ))}
                        </div>
                    ) : (
                        <div className={estilo.status_message}>Ainda não tem nenhuma avaliação.</div>
                    )}
                </div>
            </TopBar>
        </div>
    );
}

export default Home;
