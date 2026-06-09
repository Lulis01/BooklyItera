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
        carregarFeed();
    }, []);

    async function carregarFeed() {
        try {
            setEstaCarregando(true);

            const avaliacoes = await AvaliacaoAPI.listarAsync();
            const usuarios = await UsuarioAPI.listarAsync();
            const livros = await LivroAPI.listarAsync();
            const curtidas = await CurtidaAPI.listarAsync();
            const comentarios = await ComentarioAPI.listarAsync();

            const mapaUsuarios = {};
            for (const user of usuarios) {
                mapaUsuarios[user.id] = user;
            }

            const mapaLivros = {};
            for (const book of livros) {
                mapaLivros[book.id] = book;
            }

            const feedCompleto = [];
            for (const review of avaliacoes) {
                const curtidasDaAvaliacao = curtidas.filter(c => c.avaliacaoId === review.id);
                const comentariosDaAvaliacao = comentarios.filter(c => c.avaliacaoId === review.id);

                feedCompleto.push({
                    review,
                    user: mapaUsuarios[review.usuarioId],
                    book: mapaLivros[review.livroId],
                    likes: curtidasDaAvaliacao,
                    comments: comentariosDaAvaliacao,
                    userMap: mapaUsuarios
                });
            }

            feedCompleto.sort((a, b) => new Date(b.review.dataCriacao) - new Date(a.review.dataCriacao));
            setDadosDoFeed(feedCompleto);

        } catch (erro) {
            console.error(erro);
            setMensagemErro("Não foi possível carregar o feed agora.");
        } finally {
            setEstaCarregando(false);
        }
    }

    let conteudo;
    if (estaCarregando) {
        conteudo = <div className={estilo.status_message}>Carregando o feed...</div>;
    } else if (mensagemErro) {
        conteudo = <div className={estilo.error_message}>{mensagemErro}</div>;
    } else if (dadosDoFeed.length > 0) {
        conteudo = (
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
        );
    } else {
        conteudo = <div className={estilo.status_message}>Ainda não tem nenhuma avaliação.</div>;
    }

    return (
        <div className={estilo.conteudo}>
            <TopBar>
                <div className={estilo.pagina_conteudo}>
                    <div className={estilo.feed_header}>
                        <h2>Feed de Atividade</h2>
                        <p>Veja o que a comunidade está lendo e avaliando.</p>
                    </div>

                    {conteudo}
                </div>
            </TopBar>
        </div>
    );
}

export default Home;
