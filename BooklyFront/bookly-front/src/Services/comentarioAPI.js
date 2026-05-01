import { HTTPClient } from "./client";

const ComentarioAPI = {
  async listarAsync() {
    try {
      const response = await HTTPClient.get("/api/Comentario/Listar");
      return response.data;
    } catch (error) {
      console.error("Erro ao listar comentários:", error);
      throw error;
    }
  },

  async obterPorIdAsync(id) {
    try {
      const response = await HTTPClient.get(`/api/Comentario/Obter/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao obter comentário:", error);
      throw error;
    }
  },

  async criarAsync(usuarioId, avaliacaoId, texto) {
    try {
      const response = await HTTPClient.post("/api/Comentario/Criar", {
        usuarioId,
        avaliacaoId,
        texto
      });
      return response.data;
    } catch (error) {
      console.error("Erro ao criar comentário:", error);
      throw error;
    }
  },

  async atualizarAsync(id, texto) {
    try {
      const response = await HTTPClient.put(`/api/Comentario/Atualizar/${id}`, {
        texto
      });
      return response.data;
    } catch (error) {
      console.error("Erro ao atualizar comentário:", error);
      throw error;
    }
  },

  async deletarAsync(id) {
    try {
      const response = await HTTPClient.delete(`/api/Comentario/Deletar/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao deletar comentário:", error);
      throw error;
    }
  }
};

export default ComentarioAPI;
