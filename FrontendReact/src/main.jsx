import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css'; 
import { BrowserRouter } from 'react-router-dom';
import App from './App';

const root = ReactDOM.createRoot(document.getElementById('app'));
root.render(
    <React.StrictMode>
        <BrowserRouter>  {/* Envuelve App con BrowserRouter */}
            <App />
        </BrowserRouter>
    </React.StrictMode>
);
