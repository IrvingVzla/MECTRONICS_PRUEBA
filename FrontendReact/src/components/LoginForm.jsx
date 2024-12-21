import React, { useState } from 'react';
import { Link } from 'react-router-dom'; 
import Swal from 'sweetalert2';
import ReactLoading from 'react-loading';
import { apiUrl } from '../config';
import { useNavigate } from 'react-router-dom';

const LoginForm = () => {
    const [correo, setCorreo] = useState('');
    const [contrasena, setContrasena] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);

        // Objeto para enviar al backend
        const login = {
            CORREO_ELECTRONICO: correo,
            CONTRASENA: contrasena
        };

        try {
            const response = await fetch(`${apiUrl}/api/Login/iniciarSesion`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(login)
            });

            if (response.ok) {
                const data = await response.json();
                
                localStorage.setItem('token', data.token);
                localStorage.setItem('nombre', data.nombre);

                navigate('/PaginaPrincipal');

            } else {
                throw new Error(await response.text());
            }

        } catch (error) {
            Swal.fire({
                title: 'Error',
                text: error.message,
                icon: 'error',
                confirmButtonText: 'Intentar de nuevo'
            });

        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container">
            <div className="form-container">
                <h2 className="text-center mb-4">Iniciar Sesión</h2>
                <form onSubmit={handleSubmit} className="mt-4">
                    <div className="mb-3">
                        <label htmlFor="correo" className="form-label">Usuario</label>
                        <input
                            type="text"
                            id="correo"
                            className="form-control"
                            value={correo}
                            onChange={(e) => setCorreo(e.target.value)}
                            placeholder="Correo Electrónico"
                        />
                    </div>

                    <div className="mb-3">
                        <label htmlFor="contrasena" className="form-label">Contraseña</label>
                        <input
                            type="password"
                            id="contrasena"
                            className="form-control"
                            value={contrasena}
                            onChange={(e) => setContrasena(e.target.value)}
                            placeholder="Contraseña"
                        />
                    </div>

                    {loading && (
                        <div className="overlay">
                            <div className="spinner-container">
                                <ReactLoading type="spin" color="#007bff" height={100} width={100} />
                            </div>
                        </div>
                    )}

                    <div className="d-grid gap-2">
                        <button type="submit" className="btn btn-primary">Iniciar sesión</button>
                    </div>
                </form>

                <div className="text-center mt-3">
                    <p>¿No tienes cuenta? <Link to="/FormularioRegistro">Regístrate aquí</Link></p>  {/* Usar Link para navegación */}
                </div>
            </div>
        </div>
    );
};

export default LoginForm;
