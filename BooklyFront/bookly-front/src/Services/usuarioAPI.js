import { HTTPClient } from "./client";

const UsuarioAPI = {
  async listarAsync() {
    try {
      const response = await HTTPClient.get("/api/Usuario/Listar");
      return response.data;
    } catch (error) {
      console.error("Erro ao listar usuários:", error);
      throw error;
    }
  },

  async obterPorIdAsync(id) {
    try {
      const response = await HTTPClient.get(`/api/Usuario/Obter/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao obter usuário:", error);
      throw error;
    }
  },

  async criarAsync(usuarioData) {
    try {
      const response = await HTTPClient.post("/api/Usuario/Criar", usuarioData);
      return response.data;
    } catch (error) {
      console.error("Erro ao criar usuário:", error);
      throw error;
    }
  },

  async loginAsync(email, senha) {
    try {
      const response = await HTTPClient.post("/api/Usuario/Login", { Email: email, Senha: senha });
      return response.data;
    } catch (error) {
      console.error("Erro ao realizar login:", error);
      throw error;
    }
  },

  async refreshAsync() {
    try {
      const response = await HTTPClient.post("/api/Usuario/Refresh");
      return response.data;
    } catch (error) {
      console.error("Erro ao atualizar token:", error);
      throw error;
    }
  },

  async atualizarAsync(id, usuarioData) {
    try {
      const response = await HTTPClient.put(`/api/Usuario/Atualizar/${id}`, usuarioData);
      return response.data;
    } catch (error) {
      console.error("Erro ao atualizar usuário:", error);
      throw error;
    }
  },

  async deletarAsync(id) {
    try {
      const response = await HTTPClient.delete(`/api/Usuario/Deletar/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao deletar usuário:", error);
      throw error;
    }
  }
};

export default UsuarioAPI;
