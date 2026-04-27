import { HTTPClient } from "./client";

const AvaliacaoAPI = {
  async listarAsync() {
    try {
      const response = await HTTPClient.get("/api/Avaliacao/Listar");
      return response.data;
    } catch (error) {
      console.error("Erro ao listar avaliações:", error);
      throw error;
    }
  },

  async obterPorIdAsync(id) {
    try {
      const response = await HTTPClient.get(`/api/Avaliacao/Obter/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao obter avaliação:", error);
      throw error;
    }
  },

  async criarAsync(usuarioId, livroId, texto, nota) {
    try {
      const response = await HTTPClient.post("/api/Avaliacao/Criar", {
        usuarioId,
        livroId,
        texto,
        nota
      });
      return response.data;
    } catch (error) {
      console.error("Erro ao criar avaliação:", error);
      throw error;
    }
  },

  async atualizarAsync(id, texto, nota) {
    try {
      const response = await HTTPClient.put(`/api/Avaliacao/Atualizar/${id}`, {
        texto,
        nota
      });
      return response.data;
    } catch (error) {
      console.error("Erro ao atualizar avaliação:", error);
      throw error;
    }
  },

  async deletarAsync(id) {
    try {
      const response = await HTTPClient.delete(`/api/Avaliacao/Deletar/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao deletar avaliação:", error);
      throw error;
    }
  }
};

export default AvaliacaoAPI;
