import { HTTPClient } from "./client";

const LivroAPI = {
  async listarAsync() {
    try {
      const response = await HTTPClient.get("/api/Livro/Listar");
      return response.data;
    } catch (error) {
      console.error("Erro ao listar livros:", error);
      throw error;
    }
  },

  async obterPorIdAsync(id) {
    try {
      const response = await HTTPClient.get(`/api/Livro/Obter/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao obter livro:", error);
      throw error;
    }
  },

  async buscarAsync(titulo) {
    try {
      const response = await HTTPClient.get(`/api/Livro/Buscar?titulo=${titulo}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao buscar livros externos:", error);
      throw error;
    }
  },

  async importarAsync(titulo) {
    try {
      const response = await HTTPClient.post(`/api/Livro/Importar?titulo=${titulo}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao importar livros:", error);
      throw error;
    }
  },

  async criarAsync(livroData) {
    try {
      const response = await HTTPClient.post("/api/Livro/Criar", livroData);
      return response.data;
    } catch (error) {
      console.error("Erro ao criar livro:", error);
      throw error;
    }
  },

  async atualizarAsync(id, livroData) {
    try {
      const response = await HTTPClient.put(`/api/Livro/Atualizar/${id}`, livroData);
      return response.data;
    } catch (error) {
      console.error("Erro ao atualizar livro:", error);
      throw error;
    }
  },

  async deletarAsync(id) {
    try {
      const response = await HTTPClient.delete(`/api/Livro/Deletar/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao deletar livro:", error);
      throw error;
    }
  }
};

export default LivroAPI;
