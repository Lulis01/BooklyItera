import style from './TopBar.module.css'
import Logo from "../../Assets/LogoTopBar.png"
import { TopBarItem } from '../TopBarItem/TopBarItem'
import { Link } from 'react-router-dom'
import { useAuth } from '../../Context/AuthContext'


export function TopBar({ children }) {
  const { isAuthenticated, logout } = useAuth();

  return (
    <div>
      <div className={style.topbar_conteudo}>
        <div className={style.topbar_header}>
          <Link to="/home">
            <img src={Logo} alt="LogoTopBar" className={style.logo} />
          </Link>
        </div>
        <div className={style.topbar_corpo}>
          {isAuthenticated && <TopBarItem texto="Avaliar" link="/avaliar" />}
          {isAuthenticated && <TopBarItem texto="Recomendações" link="/recomendacao" />}
          {!isAuthenticated ? (
            <>
              <TopBarItem texto="Login" link="/login" />
              <TopBarItem texto="Cadastre-se" link="/cadastro" />
            </>
          ) : (
            <button onClick={logout} className={style.botao_sair}>Sair</button>
          )}
        </div>
      </div>
      <div className={style.pagina_conteudo}>{children}</div>
    </div>
  );
}
