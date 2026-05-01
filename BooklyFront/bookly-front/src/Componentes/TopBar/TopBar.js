import estilo from './TopBar.module.css'
import Logo from "../../Assets/LogoTopBar.png"
import { TopBarItem } from '../TopBarItem/TopBarItem'
import { Link } from 'react-router-dom'
import { useAuth } from '../../Context/AuthContext'


export function TopBar({ children }) {
  const { isAuthenticated, logout } = useAuth();

  return (
    <div>
      <div className={estilo.topbar_conteudo}>
        <div className={estilo.topbar_header}>
          <Link to="/home">
            <img src={Logo} alt="LogoTopBar" className={estilo.logo} />
          </Link>
        </div>
        <div className={estilo.topbar_corpo}>
          {isAuthenticated && <TopBarItem texto="Avaliar" link="/avaliar" />}
          {isAuthenticated && <TopBarItem texto="Minhas Avaliações" link="/minhas-avaliacoes" />}
          {isAuthenticated && <TopBarItem texto="Recomendações" link="/recomendacao" />}
          
          {isAuthenticated === false ? (
            <>
              <TopBarItem texto="Login" link="/login" />
              <TopBarItem texto="Cadastre-se" link="/cadastro" />
            </>
          ) : (
            <button onClick={logout} className={estilo.botao_sair}>Sair</button>
          )}
        </div>
      </div>
      <div className={estilo.pagina_conteudo}>{children}</div>
    </div>
  );
}
