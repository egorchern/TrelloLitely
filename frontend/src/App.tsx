import './App.css';
import styles from './App.module.css'
import { Link, Route, Routes } from 'react-router-dom';
import { Home } from './pages/Home/Home';
import { AuthenticationPage } from './pages/Login/AuthenticationPage';
import { Pages } from './types/pages';

function App() {

  return (
    <>
      <div className={styles.app}>
        <div className={styles.appHeader}>
          <h1>TrelloLitely</h1>
          <nav className={styles.pageNav}>
            <Link to={Pages.Home}>Home</Link>
            <Link to={Pages.Authentication}>Login</Link>
          </nav>
          
        </div>
        <Routes>
            <Route path={Pages.Home} element={<Home />} />
            <Route path={Pages.Authentication} element={<AuthenticationPage />} />
          </Routes>
      </div>
    </>
  )
}

export default App
