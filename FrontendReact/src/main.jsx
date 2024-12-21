// src/main.jsx
import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';  // Si tienes estilos globales
import { BrowserRouter } from 'react-router-dom';  // Importa BrowserRouter
import App from './App';

const root = ReactDOM.createRoot(document.getElementById('app'));
root.render(
    <React.StrictMode>
        <BrowserRouter>  {/* Envuelve App con BrowserRouter */}
            <App />
        </BrowserRouter>
    </React.StrictMode>
);
