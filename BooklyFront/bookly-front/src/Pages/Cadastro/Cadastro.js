import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import style from "./Cadastro.module.css";
import UsuarioAPI from "../../Services/usuarioAPI";
import logo from "../../Assets/LogoLogin.png";

export function Cadastro() {
    const navigate = useNavigate();
    const [nome, setNome] = useState("");
    const [email, setEmail] = useState("");
    const [senha, setSenha] = useState("");
    const [confirmarSenha, setConfirmarSenha] = useState("");
    const [erro, setErro] = useState("");
    const [carregando, setCarregando] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErro("");

        if (senha !== confirmarSenha) {
            setErro("As senhas não coincidem.");
            return;
        }

        setCarregando(true);

        try {
            await UsuarioAPI.criarAsync({
                Nome: nome,
                Email: email,
                SenhaHash: senha // O backend faz o hash lá, mas o campo se chama SenhaHash no DTO
            });
            alert("Conta criada com sucesso! Faça login para continuar.");
            navigate("/login");
        } catch (error) {
            setErro(error.response?.data?.mensagem || "Erro ao criar conta. Verifique os dados e tente novamente.");
        } finally {
            setCarregando(false);
        }
    };

    return (
        <div className={style.pagina_conteudo}>
            <img src={logo} alt="Bookly" className={style.logo} />

            <Form className={style.formulario} onSubmit={handleSubmit}>
                <h3 className="text-center mb-4">Crie sua conta</h3>
                
                {erro && <div className="alert alert-danger">{erro}</div>}

                <Form.Group className="mb-3" controlId="formNome">
                    <Form.Label>Nome Completo</Form.Label>
                    <Form.Control
                        type="text"
                        placeholder="Seu nome"
                        value={nome}
                        onChange={(e) => setNome(e.target.value)}
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formEmail">
                    <Form.Label>Email</Form.Label>
                    <Form.Control
                        type="email"
                        placeholder="Ex: joao@email.com"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formSenha">
                    <Form.Label>Senha</Form.Label>
                    <Form.Control
                        type="password"
                        placeholder="Mínimo 6 caracteres"
                        value={senha}
                        onChange={(e) => setSenha(e.target.value)}
                        required
                        minLength={6}
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formConfirmarSenha">
                    <Form.Label>Confirmar Senha</Form.Label>
                    <Form.Control
                        type="password"
                        placeholder="Repita sua senha"
                        value={confirmarSenha}
                        onChange={(e) => setConfirmarSenha(e.target.value)}
                        required
                    />
                </Form.Group>

                <Button 
                    type="submit" 
                    className={style.botao}
                    disabled={carregando}
                >
                    {carregando ? "Criando conta..." : "Cadastrar"}
                </Button>

                <div className={style.login_link}>
                    Já tem uma conta? <Link to="/login">Entrar</Link>
                </div>
            </Form>
        </div>
    );
}

export default Cadastro;
