import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import Swal from 'sweetalert2';
import ReactLoading from 'react-loading';
import { apiUrl } from '../../config';
import { useNavigate } from 'react-router-dom';
import './FormularioRegistro.css';

const RegistroForm = () => {
    const [nombre, setNombre] = useState('');
    const [apellido, setApellido] = useState('');
    const [correo, setCorreo] = useState('');
    const [contrasena, setContrasena] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);

        // Objeto para enviar al backend
        const estudiante = {
            NOMBRE: nombre,
            APELLIDO: apellido,
            CORREO_ELECTRONICO: correo,
            CONTRASENA: contrasena
        };

        try {
            // Realizamos la solicitud POST a la API
            const response = await fetch(`${apiUrl}/api/Estudiantes/crearEstudiante`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(estudiante)
            });

            if (response.ok) {
                Swal.fire({
                    title: '¡Éxito!',
                    text: 'Estudiante creado con éxito.',
                    icon: 'success',
                    confirmButtonText: 'Aceptar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        navigate('/');
                    }
                });

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
                <h2 className="text-center mb-4">Formulario de Registro</h2>
                <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                        <label htmlFor="nombre" className="form-label">Nombre</label>
                        <input type="text" className="form-control" id="nombre" value={nombre} onChange={(e) => setNombre(e.target.value)} required placeholder="Nombres"/>
                    </div>

                    <div className="mb-3">
                        <label htmlFor="apellido" className="form-label">Apellido</label>
                        <input type="text" className="form-control" id="apellido" value={apellido} onChange={(e) => setApellido(e.target.value)} required placeholder="Apellidos"/>
                    </div>

                    <div className="mb-3">
                        <label htmlFor="correo" className="form-label">Correo Electrónico</label>
                        <input type="email" className="form-control" id="correo" value={correo} onChange={(e) => setCorreo(e.target.value)} required placeholder="Correo Electrónico" />
                    </div>

                    <div className="mb-3">
                        <label htmlFor="contrasena" className="form-label">Contraseña</label>
                        <input type="password" className="form-control" id="contrasena" value={contrasena} onChange={(e) => setContrasena(e.target.value)} required placeholder="Contraseña" />
                    </div>

                    {loading && (
                        <div className="overlay">
                            <div className="spinner-container">
                                <ReactLoading type="spin" color="#007bff" height={100} width={100} />
                            </div>
                        </div>
                    )}

                    <div className="d-grid gap-2">
                        <button type="submit" className="btn btn-primary">Registrar</button>
                    </div>
                </form>

                <div className="text-center mt-3">
                    <p>¿Ya tienes una cuenta? <Link to="/">Inicia sesión</Link></p>  {/* Usar Link para navegación */}
                </div>
            </div>
        </div>
    );
};

export default RegistroForm;
