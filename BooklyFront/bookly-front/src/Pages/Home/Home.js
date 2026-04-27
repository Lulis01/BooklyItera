import { TopBar } from '../../Componentes/TopBar/TopBar'
import style from './Home.module.css';

function Home(){
    return(
        <div className ={style.conteudo}>
        <TopBar>
            <div className={style.pagina_conteudo}>
            <h3>Home</h3>
            </div>
        </TopBar>
        </div>
    );
}

export default Home;