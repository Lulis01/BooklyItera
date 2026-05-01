import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import estilo from "./Login.module.css";
import UsuarioAPI from "../../Services/usuarioAPI";
import logo from "../../Assets/LogoLogin.png";
import { useAuth } from "../../Context/AuthContext";

export function Login() {
    const navegar = useNavigate();
    const { login } = useAuth();
    const [emailDigitado, setEmailDigitado] = useState("");
    const [senhaDigitada, setSenhaDigitada] = useState("");
    const [mensagemErro, setMensagemErro] = useState("");
    const [estaCarregando, setEstaCarregando] = useState(false);

    const aoEnviar = async (evento) => {
        evento.preventDefault();
        setMensagemErro("");
        setEstaCarregando(true);

        try {
            const dadosDoUsuario = await UsuarioAPI.loginAsync(emailDigitado, senhaDigitada);
            login(dadosDoUsuario); 
            navegar("/home");
        } catch (erro) {
            const erroFormatado = erro.response?.data?.mensagem || erro.message || "Erro ao fazer login.";
            setMensagemErro(erroFormatado);
        } finally {
            setEstaCarregando(false);
        }
    };

    return (
        <div className={estilo.pagina_conteudo}>
            <img src={logo} alt="Bookly" className={estilo.logo} />

            <Form className={estilo.formulario} onSubmit={aoEnviar}>
                <h3 className="text-center mb-4">Bem-vindo de volta!</h3>
                
                {mensagemErro !== "" && <div className="alert alert-danger">{mensagemErro}</div>}

                <Form.Group className="mb-3" controlId="formBasicEmail">
                    <Form.Control
                        type="email"
                        placeholder="Digite seu email"
                        value={emailDigitado}
                        onChange={(e) => setEmailDigitado(e.target.value)}
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formBasicPassword">
                    <Form.Control
                        type="password"
                        placeholder="Digite sua senha"
                        value={senhaDigitada}
                        onChange={(e) => setSenhaDigitada(e.target.value)}
                        required
                    />
                </Form.Group>

                <Button 
                    type="submit" 
                    className={estilo.botao}
                    disabled={estaCarregando}
                >
                    {estaCarregando === true ? "Entrando..." : "Entrar"}
                </Button>

                <div className={estilo.cadastro_link}>
                    Não tem uma conta? <Link to="/cadastro">Cadastre-se</Link>
                </div>
            </Form>
        </div>
    );
}

export default Login;
