import React, { useState, useEffect } from 'react';
import styles from './ReviewCard.module.css';
import { useAuth } from '../../Context/AuthContext';
import ComentarioAPI from '../../Services/comentarioAPI';
import CurtidaAPI from '../../Services/curtidaAPI';

const ReviewCard = ({ review, user: reviewUser, book, likes: initialLikes, comments: initialComments, userMap }) => {
    const { user: currentUser, isAuthenticated } = useAuth();
    const [comments, setComments] = useState(initialComments || []);
    const [likes, setLikes] = useState(initialLikes || []);
    const [newComment, setNewComment] = useState('');
    const [submitting, setSubmitting] = useState(false);
    const [showAllComments, setShowAllComments] = useState(false);
    const [isLiked, setIsLiked] = useState(false);

    useEffect(() => {
        if (currentUser && likes) {
            setIsLiked(likes.some(l => l.usuarioId === currentUser.id));
        }
    }, [currentUser, likes]);

    const renderStars = (nota) => {
        return '★'.repeat(nota) + '☆'.repeat(5 - nota);
    };

    const handleLike = async () => {
        if (!isAuthenticated) {
            alert("Faça login para curtir!");
            return;
        }

        try {
            if (isLiked) {
                const curtida = likes.find(l => l.usuarioId === currentUser.id);
                if (curtida) {
                    await CurtidaAPI.deletarAsync(curtida.id);
                    setLikes(likes.filter(l => l.id !== curtida.id));
                }
            } else {
                const novaCurtida = await CurtidaAPI.criarAsync(currentUser.id, review.id);
                setLikes([...likes, novaCurtida]);
            }
            setIsLiked(!isLiked);
        } catch (error) {
            console.error("Erro ao processar curtida:", error);
        }
    };

    const handleCommentSubmit = async (e) => {
        e.preventDefault();
        if (!newComment.trim() || !currentUser) return;

        try {
            setSubmitting(true);
            const createdComment = await ComentarioAPI.criarAsync(
                currentUser.id,
                review.id,
                newComment
            );
            setComments([...comments, createdComment]);
            setNewComment('');
            setShowAllComments(true);
        } catch (error) {
            console.error("Erro ao enviar comentário:", error);
            alert("Não foi possível enviar o comentário.");
        } finally {
            setSubmitting(false);
        }
    };

    const displayedComments = showAllComments ? comments : comments.slice(0, 1);

    return (
        <div className={styles.card}>
            <div className={styles.header}>
                <div className={styles.avatar}>
                    {reviewUser?.nome?.charAt(0).toUpperCase() || '?'}
                </div>
                <div className={styles.userInfo}>
                    <h4 className={styles.userName}>{reviewUser?.nome || 'Usuário desconhecido'}</h4>
                    <span className={styles.date}>
                        {new Date(review.dataCriacao).toLocaleDateString('pt-BR')}
                    </span>
                </div>
                <div className={styles.rating}>
                    {renderStars(review.nota)}
                </div>
            </div>

            <div className={styles.bookInfo}>
                <span className={styles.bookLabel}>Livro:</span>
                <span className={styles.bookTitle}>{book?.titulo || 'Título não encontrado'}</span>
                <span className={styles.bookAuthor}> por {book?.autor || 'Autor desconhecido'}</span>
            </div>

            <div className={styles.content}>
                <p>{review.texto}</p>
            </div>

            <div className={styles.footer}>
                <div className={styles.interaction}>
                    <button 
                        className={`${styles.interactionBtn} ${isLiked ? styles.liked : ''}`} 
                        onClick={handleLike}
                    >
                        <span className={styles.icon}>{isLiked ? '❤️' : '❤️'}</span>
                        {likes.length} Curtidas
                    </button>
                    <button className={styles.interactionBtn} onClick={() => setShowAllComments(!showAllComments)}>
                        <span className={styles.icon}>💬</span>
                        {comments.length} Comentários
                    </button>
                </div>

                <div className={styles.commentsSection}>
                    {comments.length > 0 && (
                        <div className={styles.commentsList}>
                            {displayedComments.map(comment => (
                                <div key={comment.id} className={styles.commentItem}>
                                    <span className={styles.commentUser}>
                                        {userMap[comment.usuarioId]?.nome || 'Usuário'}:
                                    </span>
                                    <span className={styles.commentText}>{comment.texto}</span>
                                </div>
                            ))}
                            
                            {comments.length > 1 && (
                                <button 
                                    className={styles.toggleCommentsBtn}
                                    onClick={() => setShowAllComments(!showAllComments)}
                                >
                                    {showAllComments ? 'Ver menos' : `Ver todos os ${comments.length} comentários`}
                                </button>
                            )}
                        </div>
                    )}

                    {isAuthenticated ? (
                        <form onSubmit={handleCommentSubmit} className={styles.commentForm}>
                            <input 
                                type="text" 
                                placeholder="Adicione um comentário..." 
                                value={newComment}
                                onChange={(e) => setNewComment(e.target.value)}
                                className={styles.commentInput}
                                disabled={submitting}
                            />
                            <button 
                                type="submit" 
                                className={styles.commentSubmitBtn}
                                disabled={submitting || !newComment.trim()}
                            >
                                {submitting ? '...' : 'Enviar'}
                            </button>
                        </form>
                    ) : (
                        <p className={styles.loginToComment}>Faça login para interagir.</p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default ReviewCard;
