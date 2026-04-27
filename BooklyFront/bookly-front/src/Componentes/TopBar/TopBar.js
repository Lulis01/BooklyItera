import style from './TopBar.module.css'
import Logo from "../../Assets/LogoTopBar.png"
import {TopBarItem} from '../TopBarItem/TopBarItem';


export function TopBar({ children }) {
  return (
    <div>
      <div className={style.sidebar_conteudo}>
        <div className={style.sidebar_header}>
          <img link="/" src={Logo} alt="LogoTopBar" className=  {style.logo} />
          <hr className={style.linha} />
        </div>  
        <div className={style.sidebar_corpo}>
          <TopBarItem texto="Livros" link=""/>
          <TopBarItem texto="Login" link=""/>
          <TopBarItem texto="Cadastre-se"link=""/>
        </div>
      </div>
      <div className={style.pagina_conteudo}>{children}</div>
    </div>
  );
}
