import { HTTPClient } from "./client";

const CurtidaAPI = {
  async listarAsync() {
    try {
      const response = await HTTPClient.get("/api/Curtida/Listar");
      return response.data;
    } catch (error) {
      console.error("Erro ao listar curtidas:", error);
      throw error;
    }
  },

  async obterPorIdAsync(id) {
    try {
      const response = await HTTPClient.get(`/api/Curtida/Obter/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao obter curtida:", error);
      throw error;
    }
  },

  async criarAsync(usuarioId, avaliacaoId) {
    try {
      const response = await HTTPClient.post("/api/Curtida/Criar", {
        usuarioId,
        avaliacaoId
      });
      return response.data;
    } catch (error) {
      console.error("Erro ao criar curtida:", error);
      throw error;
    }
  },

  async deletarAsync(id) {
    try {
      const response = await HTTPClient.delete(`/api/Curtida/Deletar/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao deletar curtida:", error);
      throw error;
    }
  }
};

export default CurtidaAPI;
