import './App.css';
import styles from './App.module.css'
import { Link, Route, Routes } from 'react-router-dom';
import { Home } from './pages/Home/Home';
import { AuthenticationPage } from './pages/Login/AuthenticationPage';
import { Pages } from './types/pages';
import { AuthenticationContextProvider } from './components/AuthenticationContext';
import BoardPage from './pages/Board/BoardPage';
import { Input } from './components/ui/input';
import { GlobalFilterContext, GlobalFilterContextProvider } from './components/GlobalFilterContext';
import { GlobalFilter } from './components/GlobalFilter';

function App() {

  return (
    <>
    <AuthenticationContextProvider>
      <GlobalFilterContextProvider>
        <div className={styles.app}>
          <div className={styles.appHeader}>
            <h1>TrelloLitely</h1>
            <nav className={styles.pageNav}>
              <GlobalFilter />
              <Link to={Pages.Home}>Home</Link>
              <Link to={Pages.Authentication}>Login</Link>
              <Link to={Pages.Board.replace(':boardId', '1')}>Board</Link>
            </nav>
            
          </div>
          <Routes>
              <Route path={Pages.Home} element={<Home />} />
              <Route path={Pages.Authentication} element={<AuthenticationPage />} />
              <Route path={Pages.Board} element={<BoardPage />} />
            </Routes>
        </div>
      </GlobalFilterContextProvider>
    </AuthenticationContextProvider>
    </>
  )
}

export default App
