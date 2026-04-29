import { useState, useRef, useEffect } from "react";
import ChatbotAPI from "../../Services/chatbotAPI";
import { TopBar } from "../../Componentes/TopBar/TopBar";
import styles from "./Chatbot.module.css";

const SUGESTOES = [
  "Me recomenda livros parecidos com Harry Potter 🧙",
  "Quero algo triste e emocionante 😢",
  "Livros de fantasia medieval ⚔️",
  "Gostei de Percy Jackson, o que ler agora?",
  "Me recomenda suspense curto 🔍",
  "Quero sair da minha zona de conforto 🌍",
];

function Chatbot() {
  const [mensagens, setMensagens] = useState([
    {
      tipo: "bot",
      texto: "Olá! Sou o **Bookly Bot** 📚 Seu guia literário pessoal! Me diga o que você quer ler e eu vou te indicar os melhores livros. O que você está procurando?",
      recomendacoes: [],
    },
  ]);
  const [input, setInput] = useState("");
  const [carregando, setCarregando] = useState(false);
  const fimRef = useRef(null);

  useEffect(() => {
    fimRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [mensagens, carregando]);

  async function enviar(textoOverride) {
    const texto = textoOverride ?? input.trim();
    if (!texto || carregando) return;

    const novaMensagem = { tipo: "user", texto, recomendacoes: [] };
    setMensagens((prev) => [...prev, novaMensagem]);
    setInput("");
    setCarregando(true);

    try {
      const resposta = await ChatbotAPI.enviarMensagemAsync(texto);
      setMensagens((prev) => [
        ...prev,
        {
          tipo: "bot",
          texto: resposta.mensagem,
          recomendacoes: resposta.recomendacoes || [],
        },
      ]);
    } catch {
      setMensagens((prev) => [
        ...prev,
        {
          tipo: "bot",
          texto: "Desculpe, tive um problema ao buscar recomendações. Tente novamente! 😅",
          recomendacoes: [],
        },
      ]);
    } finally {
      setCarregando(false);
    }
  }

  function handleKey(e) {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      enviar();
    }
  }

  return (
    <div>
      <TopBar />
      <div className={styles.pagina}>
        {/* Coluna lateral com sugestões */}
        <aside className={styles.sidebar}>
          <h3 className={styles.sidebar_titulo}>💡 Experimente perguntar</h3>
          <ul className={styles.sugestoes}>
            {SUGESTOES.map((s, i) => (
              <li key={i}>
                <button
                  className={styles.sugestao_btn}
                  onClick={() => enviar(s)}
                  disabled={carregando}
                >
                  {s}
                </button>
              </li>
            ))}
          </ul>
        </aside>

        {/* Área principal do chat */}
        <div className={styles.chat_wrapper}>
          <div className={styles.chat_area}>
            {mensagens.map((msg, i) => (
              <div
                key={i}
                className={`${styles.mensagem} ${
                  msg.tipo === "user" ? styles.mensagem_user : styles.mensagem_bot
                }`}
              >
                {msg.tipo === "bot" && (
                  <div className={styles.avatar}>📚</div>
                )}
                <div className={styles.balao}>
                  <p className={styles.balao_texto}>{msg.texto}</p>

                  {msg.recomendacoes.length > 0 && (
                    <div className={styles.recomendacoes}>
                      {msg.recomendacoes.map((livro, j) => (
                        <div key={j} className={styles.livro_card}>
                          <div className={styles.livro_num}>{j + 1}</div>
                          <div>
                            <p className={styles.livro_titulo}>{livro.titulo}</p>
                            <p className={styles.livro_autor}>{livro.autor}</p>
                            <p className={styles.livro_motivo}>💡 {livro.motivo}</p>
                          </div>
                        </div>
                      ))}
                    </div>
                  )}
                </div>
                {msg.tipo === "user" && (
                  <div className={styles.avatar_user}>👤</div>
                )}
              </div>
            ))}

            {carregando && (
              <div className={`${styles.mensagem} ${styles.mensagem_bot}`}>
                <div className={styles.avatar}>📚</div>
                <div className={styles.balao}>
                  <div className={styles.typing}>
                    <span></span><span></span><span></span>
                  </div>
                </div>
              </div>
            )}
            <div ref={fimRef} />
          </div>

          {/* Input */}
          <div className={styles.input_area}>
            <textarea
              className={styles.input}
              placeholder="Pergunte ao Bookly Bot... Ex: Me recomenda livros de ficção científica 🚀"
              value={input}
              onChange={(e) => setInput(e.target.value)}
              onKeyDown={handleKey}
              rows={2}
              disabled={carregando}
            />
            <button
              className={styles.btn_enviar}
              onClick={() => enviar()}
              disabled={carregando || !input.trim()}
            >
              ➤
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Chatbot;
