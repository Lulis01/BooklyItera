import { useState, useRef, useEffect } from "react";
import iaAPI from "../../Services/iaAPI";
import { TopBar } from "../../Componentes/TopBar/TopBar";
import estilo from "./Recomendacao.module.css";

const SUGESTOES = [
  "Me recomenda livros parecidos com Harry Potter 🧙",
  "Quero algo triste e emocionante 😢",
  "Livros de fantasia medieval ⚔️",
  "Gostei de Percy Jackson, o que ler agora?",
  "Me recomenda suspense curto 🔍",
  "Quero sair da minha zona de conforto 🌍",
];

function Recomendacao() {
  const [mensagens, setMensagens] = useState([
    {
      tipo: "bot",
      texto: "Olá! Sou o **Bookly Bot** 📚 Seu guia literário pessoal! Me diga o que você quer ler e eu vou te indicar os melhores livros. O que você está procurando?",
      recomendacoes: [],
    },
  ]);
  const [campoTexto, setCampoTexto] = useState("");
  const [carregando, setCarregando] = useState(false);
  const fimDoChatRef = useRef(null);

  useEffect(() => {
    fimDoChatRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [mensagens, carregando]);

  async function enviarMensagem(textoParaEnviar) {
    const texto = textoParaEnviar ?? campoTexto.trim();
    
    if (texto === "" || carregando === true) return;

    const novaMensagemUsuario = { tipo: "user", texto: texto, recomendacoes: [] };
    setMensagens((listaAnterior) => [...listaAnterior, novaMensagemUsuario]);
    setCampoTexto("");
    setCarregando(true);

    try {
      const respostaDoServidor = await iaAPI.enviarMensagemAoChat(texto);
      
      const novaMensagemBot = {
        tipo: "bot",
        texto: respostaDoServidor.mensagem,
        recomendacoes: respostaDoServidor.recomendacoes || [],
      };

      setMensagens((listaAnterior) => [...listaAnterior, novaMensagemBot]);
    } catch (erro) {
      const mensagemErro = {
        tipo: "bot",
        texto: "Desculpe, tive um problema ao buscar recomendações. Tente novamente! 😅",
        recomendacoes: [],
      };
      setMensagens((listaAnterior) => [...listaAnterior, mensagemErro]);
    } finally {
      setCarregando(false);
    }
  }

  function apertouTecla(evento) {
    if (evento.key === "Enter" && evento.shiftKey === false) {
      evento.preventDefault();
      enviarMensagem();
    }
  }

  return (
    <div>
      <TopBar />
      <div className={estilo.pagina}>
        <aside className={estilo.sidebar}>
          <h3 className={estilo.sidebar_titulo}>💡 Dicas de perguntas</h3>
          <ul className={estilo.sugestoes}>
            {SUGESTOES.map((dica, index) => (
              <li key={index}>
                <button
                  className={estilo.sugestao_btn}
                  onClick={() => enviarMensagem(dica)}
                  disabled={carregando}
                >
                  {dica}
                </button>
              </li>
            ))}
          </ul>
        </aside>

        <div className={estilo.chat_wrapper}>
          <div className={estilo.chat_area}>
            {mensagens.map((msg, index) => (
              <div
                key={index}
                className={`${estilo.mensagem} ${
                  msg.tipo === "user" ? estilo.mensagem_user : estilo.mensagem_bot
                }`}
              >
                {msg.tipo === "bot" && <div className={estilo.avatar}>📚</div>}
                
                <div className={estilo.balao}>
                  <p className={estilo.balao_texto}>{msg.texto}</p>

                  {msg.recomendacoes.length > 0 && (
                    <div className={estilo.recomendacoes}>
                      {msg.recomendacoes.map((livro, indexLivro) => (
                        <div key={indexLivro} className={estilo.livro_card}>
                          <div className={estilo.livro_num}>{indexLivro + 1}</div>
                          <div>
                            <p className={estilo.livro_titulo}>{livro.titulo}</p>
                            <p className={estilo.livro_autor}>{livro.autor}</p>
                            <p className={estilo.livro_motivo}>💡 {livro.motivo}</p>
                          </div>
                        </div>
                      ))}
                    </div>
                  )}
                </div>

                {msg.tipo === "user" && <div className={estilo.avatar_user}>👤</div>}
              </div>
            ))}

            {carregando && (
              <div className={`${estilo.mensagem} ${estilo.mensagem_bot}`}>
                <div className={estilo.avatar}>📚</div>
                <div className={estilo.balao}>
                  <div className={estilo.typing}>
                    <span></span><span></span><span></span>
                  </div>
                </div>
              </div>
            )}
            
            <div ref={fimDoChatRef} />
          </div>

          <div className={estilo.input_area}>
            <textarea
              className={estilo.input}
              placeholder="Pergunte ao Bookly Bot... Ex: Me recomenda livros de ficção científica 🚀"
              value={campoTexto}
              onChange={(e) => setCampoTexto(e.target.value)}
              onKeyDown={apertouTecla}
              rows={2}
              disabled={carregando}
            />
            <button
              className={estilo.btn_enviar}
              onClick={() => enviarMensagem()}
              disabled={carregando || campoTexto.trim() === ""}
            >
              ➤
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Recomendacao;
