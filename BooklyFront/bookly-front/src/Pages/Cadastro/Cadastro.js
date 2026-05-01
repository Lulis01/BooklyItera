import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import estilo from "./Cadastro.module.css";
import UsuarioAPI from "../../Services/usuarioAPI";
import logo from "../../Assets/LogoLogin.png";

export function Cadastro() {
    const navegar = useNavigate();
    const [nomeDigitado, setNomeDigitado] = useState("");
    const [emailDigitado, setEmailDigitado] = useState("");
    const [senhaDigitada, setSenhaDigitada] = useState("");
    const [confirmacaoSenha, setConfirmacaoSenha] = useState("");
    const [mensagemErro, setMensagemErro] = useState("");
    const [estaCarregando, setEstaCarregando] = useState(false);

    const aoEnviar = async (evento) => {
        evento.preventDefault();
        setMensagemErro("");

        if (senhaDigitada !== confirmacaoSenha) {
            setMensagemErro("As senhas não são iguais!");
            return;
        }

        setEstaCarregando(true);

        try {
            await UsuarioAPI.criarAsync({
                Nome: nomeDigitado,
                Email: emailDigitado,
                SenhaHash: senhaDigitada 
            });
            alert("Sua conta foi criada! Agora é só fazer o login.");
            navegar("/login");
        } catch (erro) {
            setMensagemErro(erro.response?.data?.mensagem || "Deu um erro ao criar sua conta. Tente de novo.");
        } finally {
            setEstaCarregando(false);
        }
    };

    return (
        <div className={estilo.pagina_conteudo}>
            <img src={logo} alt="Bookly" className={estilo.logo} />

            <Form className={estilo.formulario} onSubmit={aoEnviar}>
                <h3 className="text-center mb-4">Crie sua conta</h3>
                
                {mensagemErro !== "" && <div className="alert alert-danger">{mensagemErro}</div>}

                <Form.Group className="mb-3" controlId="formNome">
                    <Form.Label>Nome Completo</Form.Label>
                    <Form.Control
                        type="text"
                        placeholder="Digite seu nome"
                        value={nomeDigitado}
                        onChange={(e) => setNomeDigitado(e.target.value)}
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formEmail">
                    <Form.Label>Email</Form.Label>
                    <Form.Control
                        type="email"
                        placeholder="Ex: joao@email.com"
                        value={emailDigitado}
                        onChange={(e) => setEmailDigitado(e.target.value)}
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formSenha">
                    <Form.Label>Senha</Form.Label>
                    <Form.Control
                        type="password"
                        placeholder="Crie uma senha de 6 letras ou mais"
                        value={senhaDigitada}
                        onChange={(e) => setSenhaDigitada(e.target.value)}
                        required
                        minLength={6}
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formConfirmarSenha">
                    <Form.Label>Confirmar Senha</Form.Label>
                    <Form.Control
                        type="password"
                        placeholder="Repita a senha de cima"
                        value={confirmacaoSenha}
                        onChange={(e) => setConfirmacaoSenha(e.target.value)}
                        required
                    />
                </Form.Group>

                <Button 
                    type="submit" 
                    className={estilo.botao}
                    disabled={estaCarregando}
                >
                    {estaCarregando === true ? "Criando sua conta..." : "Cadastrar Agora"}
                </Button>

                <div className={estilo.login_link}>
                    Já tem uma conta? <Link to="/login">Entrar</Link>
                </div>
            </Form>
        </div>
    );
}

export default Cadastro;
