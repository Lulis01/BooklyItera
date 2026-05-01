import style from './TopBarItem.module.css';
import { Link } from "react-router-dom";

export function TopBarItem({texto, link, logo}){
    return(
        <Link to={link} className={style.topbar_item}>
            {logo}
            <h3 className={style.texto_link}>{texto}</h3>
        </Link>
    );
}