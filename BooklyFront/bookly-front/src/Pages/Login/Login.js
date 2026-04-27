import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import style from "./Login.module.css";
import UsuarioAPI from "../../Services/usuarioAPI";
import logo from "../../Assets/LogoLogin.png";
import { useAuth } from "../../Context/AuthContext";

export function Login() {
    const navigate = useNavigate();
    const { login } = useAuth();
    const [email, setEmail] = useState("");
    const [senha, setSenha] = useState("");
    const [erro, setErro] = useState("");
    const [carregando, setCarregando] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErro("");
        setCarregando(true);

        try {
            const data = await UsuarioAPI.loginAsync(email, senha);
            login(data); 
            navigate("/home");
        } catch (error) {
            const mensagemErro = error.response?.data?.mensagem || error.message || "Erro ao realizar login.";
            setErro(mensagemErro);
        } finally {
            setCarregando(false);
        }
    };

    return (
        <div className={style.pagina_conteudo}>
            <img src={logo} alt="Bookly" className={style.logo} />

            <Form className={style.formulario} onSubmit={handleSubmit}>
                <h3 className="text-center mb-4">Bem-vindo de volta!</h3>
                
                {erro && <div className="alert alert-danger">{erro}</div>}

                <Form.Group className="mb-3" controlId="formBasicEmail">
                    <Form.Control
                        type="email"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formBasicPassword">
                    <Form.Control
                        type="password"
                        placeholder="Senha"
                        value={senha}
                        onChange={(e) => setSenha(e.target.value)}
                        required
                    />
                </Form.Group>

                <Button 
                    type="submit" 
                    className={style.botao}
                    disabled={carregando}
                >
                    {carregando ? "Entrando..." : "Entrar"}
                </Button>

                <div className={style.cadastro_link}>
                    Não tem uma conta? <Link to="/cadastro">Cadastre-se</Link>
                </div>
            </Form>
        </div>
    );
}

export default Login;
