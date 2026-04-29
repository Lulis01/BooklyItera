import React, { useState, useEffect } from 'react';
import styles from './ReviewCard.module.css';
import { useAuth } from '../../Context/AuthContext';
import ComentarioAPI from '../../Services/comentarioAPI';
import CurtidaAPI from '../../Services/curtidaAPI';

const ReviewCard = ({ review, user, book, likes: initialLikes, comments: initialComments, userMap }) => {
    const { user: currentUser, isAuthenticated } = useAuth();

    const [listaDeComentarios, setListaDeComentarios] = useState(initialComments || []);
    const [listaDeCurtidas, setListaDeCurtidas] = useState(initialLikes || []);
    const [textoDoNovoComentario, setTextoDoNovoComentario] = useState('');
    const [estaCarregando, setEstaCarregando] = useState(false);
    const [mostrarTodosComentarios, setMostrarTodosComentarios] = useState(false);
    const [usuarioJaCurtiu, setUsuarioJaCurtiu] = useState(false);

    useEffect(() => {
        if (currentUser && listaDeCurtidas) {
            const encontrouCurtida = listaDeCurtidas.some(curtida => curtida.usuarioId === currentUser.id);
            setUsuarioJaCurtiu(encontrouCurtida);
        }
    }, [currentUser, listaDeCurtidas]);

    const mostrarEstrelas = (nota) => {
        let estrelas = '';
        for (let i = 0; i < 5; i++) {
            if (i < nota) {
                estrelas += '★';
            } else {
                estrelas += '☆';
            }
        }
        return estrelas;
    };

    const lidarComCurtida = async () => {
        if (!isAuthenticated) {
            alert("Você precisa estar logado para curtir!");
            return;
        }

        if (usuarioJaCurtiu) {
            return;
        }

        try {
            const novaCurtida = await CurtidaAPI.criarAsync(currentUser.id, review.id);
            setListaDeCurtidas([...listaDeCurtidas, novaCurtida]);
            setUsuarioJaCurtiu(true);
        } catch (erro) {
            console.log(erro);
        }
    };

    const enviarComentario = async (evento) => {
        evento.preventDefault();

        if (textoDoNovoComentario.trim() === '') {
            return;
        }

        try {
            setEstaCarregando(true);
            const comentarioCriado = await ComentarioAPI.criarAsync(
                currentUser.id,
                review.id,
                textoDoNovoComentario
            );
            
            setListaDeComentarios([...listaDeComentarios, comentarioCriado]);
            setTextoDoNovoComentario('');
            setMostrarTodosComentarios(true);
        } catch (erro) {
            console.log(erro);
            alert("Erro ao enviar comentário.");
        } finally {
            setEstaCarregando(false);
        }
    };

    let comentariosParaExibir = [];
    if (mostrarTodosComentarios) {
        comentariosParaExibir = listaDeComentarios;
    } else {
        comentariosParaExibir = listaDeComentarios.slice(0, 1);
    }

    return (
        <div className={styles.card}>
            <div className={styles.header}>
                <div className={styles.avatar}>
                    {user?.nome ? user.nome.charAt(0).toUpperCase() : '?'}
                </div>
                <div className={styles.userInfo}>
                    <h4 className={styles.userName}>{user?.nome || 'Usuário'}</h4>
                    <span className={styles.date}>
                        {new Date(review.dataCriacao).toLocaleDateString('pt-BR')}
                    </span>
                </div>
                <div className={styles.rating}>
                    {mostrarEstrelas(review.nota)}
                </div>
            </div>

            <div className={styles.bookInfo}>
                <span className={styles.bookLabel}>Livro:</span>
                <span className={styles.bookTitle}> {book?.titulo || 'Sem título'}</span>
                <span className={styles.bookAuthor}> por {book?.autor || 'Desconhecido'}</span>
            </div>

            <div className={styles.content}>
                <p>{review.texto}</p>
            </div>

            <div className={styles.footer}>
                <div className={styles.interaction}>
                    <button 
                        className={`${styles.interactionBtn} ${usuarioJaCurtiu ? styles.liked : ''}`} 
                        onClick={lidarComCurtida}
                        disabled={usuarioJaCurtiu}
                    >
                        <span className={styles.icon}>❤️</span>
                        {listaDeCurtidas.length} Curtidas
                    </button>
                    <button 
                        className={styles.interactionBtn} 
                        onClick={() => setMostrarTodosComentarios(!mostrarTodosComentarios)}
                    >
                        <span className={styles.icon}>💬</span>
                        {listaDeComentarios.length} Comentários
                    </button>
                </div>

                {mostrarTodosComentarios && (
                    <div className={styles.commentsSection}>
                        {listaDeComentarios.length > 0 && (
                            <div className={styles.commentsList}>
                                {listaDeComentarios.map(item => (
                                    <div key={item.id} className={styles.commentItem}>
                                        <span className={styles.commentUser}>
                                            {userMap[item.usuarioId]?.nome || 'Usuário'}:
                                        </span>
                                        <span className={styles.commentText}>{item.texto}</span>
                                    </div>
                                ))}
                            </div>
                        )}

                        {isAuthenticated ? (
                            <form onSubmit={enviarComentario} className={styles.commentForm}>
                                <input 
                                    type="text" 
                                    placeholder="Escreva um comentário..." 
                                    value={textoDoNovoComentario}
                                    onChange={(e) => setTextoDoNovoComentario(e.target.value)}
                                    className={styles.commentInput}
                                    disabled={estaCarregando}
                                />
                                <button 
                                    type="submit" 
                                    className={styles.commentSubmitBtn}
                                    disabled={estaCarregando || textoDoNovoComentario.trim() === ''}
                                >
                                    {estaCarregando ? '...' : 'Enviar'}
                                </button>
                            </form>
                        ) : (
                            <p className={styles.loginToComment}>Entre para comentar.</p>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
};

export default ReviewCard;
