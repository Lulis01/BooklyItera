import React, { useEffect, useState } from 'react';
import { TopBar } from '../../Componentes/TopBar/TopBar';
import ReviewCard from '../../Componentes/ReviewCard/ReviewCard';
import style from './Home.module.css';

// Import API Services
import AvaliacaoAPI from '../../Services/avaliacaoAPI';
import UsuarioAPI from '../../Services/usuarioAPI';
import LivroAPI from '../../Services/livroAPI';
import CurtidaAPI from '../../Services/curtidaAPI';
import ComentarioAPI from '../../Services/comentarioAPI';

function Home() {
    const [feedData, setFeedData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchFeed = async () => {
            try {
                setLoading(true);
                // Pega todos os dados necessários de uma vez
                const [avaliacoes, usuarios, livros, curtidas, comentarios] = await Promise.all([
                    AvaliacaoAPI.listarAsync(),
                    UsuarioAPI.listarAsync(),
                    LivroAPI.listarAsync(),
                    CurtidaAPI.listarAsync(),
                    ComentarioAPI.listarAsync()
                ]);

                // Mapeando os dados para um acesso mais fácil
                const userMap = usuarios.reduce((acc, user) => ({ ...acc, [user.id]: user }), {});
                const bookMap = livros.reduce((acc, book) => ({ ...acc, [book.id]: book }), {});

                // Combinando os dados para o feed
                const enrichedFeed = avaliacoes.map(review => {
                    const reviewLikes = curtidas.filter(c => c.avaliacaoId === review.id);
                    const reviewComments = comentarios.filter(c => c.avaliacaoId === review.id);

                    return {
                        review,
                        user: userMap[review.usuarioId],
                        book: bookMap[review.livroId],
                        likes: reviewLikes,
                        comments: reviewComments,
                        userMap // Passa o userMap para ajudar a identificar o autor do comentário
                    };
                });

                // Ordenando por dado (mais recente primeiro)
                enrichedFeed.sort((a, b) => new Date(b.review.dataCriacao) - new Date(a.review.dataCriacao));

                setFeedData(enrichedFeed);
            } catch (err) {
                console.error("Erro ao carregar o feed:", err);
                setError("Não foi possível carregar o feed. Tente novamente mais tarde.");
            } finally {
                setLoading(false);
            }
        };

        fetchFeed();
    }, []);

    return (
        <div className={style.conteudo}>
            <TopBar>
                <div className={style.pagina_conteudo}>
                    <div className={style.feed_header}>
                        <h2>Feed de Atividade</h2>
                        <p>Veja o que a comunidade está lendo e avaliando.</p>
                    </div>

                    {loading ? (
                        <div className={style.status_message}>Carregando feed...</div>
                    ) : error ? (
                        <div className={style.error_message}>{error}</div>
                    ) : feedData.length > 0 ? (
                        <div className={style.feed_container}>
                            {feedData.map(item => (
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
                        <div className={style.status_message}>Nenhuma avaliação encontrada.</div>
                    )}
                </div>
            </TopBar>
        </div>
    );
}

export default Home;
