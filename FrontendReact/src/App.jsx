// src/App.jsx
import React from 'react';
import { Routes, Route } from "react-router-dom";
import LoginForm from './components/LoginForm';
import FormularioRegistro from './components/FormularioRegistro/FormularioRegistro';
import PaginaPrincipal from './components/PaginaPrincipal/PaginaPrincipal';
import RegistroMaterias from './components/RegistroMaterias/RegistroMaterias';
import Perfil from './components/Perfil/Perfil';
import PrivateRoute from './components/PrivateRoute';

const App = () => {
    return (
        <Routes>
            {/* Rutas publicas */}
            <Route path="/" element={<LoginForm />} />
            <Route path="/FormularioRegistro" element={<FormularioRegistro />} />

            {/* Rutas protegidas */}
            <Route path="/PaginaPrincipal"  element={<PrivateRoute> <PaginaPrincipal /> </PrivateRoute>} />
            <Route path="/RegistroMaterias" element={<PrivateRoute> <RegistroMaterias /> </PrivateRoute>} />
            <Route path="/Perfil"           element={<PrivateRoute> <Perfil /> </PrivateRoute>} />
        </Routes>
    );
};

export default App;
