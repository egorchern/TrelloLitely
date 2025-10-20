import './App.css';
import styles from './App.module.css'
import React from 'react';
import { Login } from './pages/Login/Login';
import { Link, Route, Routes } from 'react-router-dom';
import { Home } from './pages/Home/Home';

function App() {

  return (
    <>
      <div className={styles.app}>
        <div className={styles.appHeader}>
          <h1>TrelloLitely</h1>
          <nav className={styles.pageNav}>
            <Link to="/">Home</Link>
            <Link to="/login">Login</Link>
          </nav>
          
        </div>
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/login" element={<Login />} />
          </Routes>
      </div>
    </>
  )
}

export default App
