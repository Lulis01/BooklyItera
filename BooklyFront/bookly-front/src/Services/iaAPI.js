import { HTTPClient } from "./client";

const iaAPI = {
  
  async enviarMensagemAoChat(textoDaMensagem) {
    try {
      const resposta = await HTTPClient.post("/api/Chatbot/Mensagem", { mensagem: textoDaMensagem });
      return resposta.data;
    } catch (erro) {
      console.error("Erro ao falar com o chatbot:", erro);
      throw erro;
    }
  },

  
  async pedirRecomendacaoPorNotas(listaDeAvaliacoes) {
    try {
      const resposta = await HTTPClient.post("/api/Recomendacao/Recomendar", { avaliacoes: listaDeAvaliacoes });
      return resposta.data;
    } catch (erro) {
      console.error("Erro ao pedir recomendações por notas:", erro);
      throw erro;
    }
  }
};

export default iaAPI;
