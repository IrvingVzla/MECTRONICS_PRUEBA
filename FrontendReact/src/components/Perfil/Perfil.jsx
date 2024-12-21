import React, { useEffect, useState, useRef } from 'react';
import Navbar from '../Navbar';
import ReactLoading from 'react-loading';
import Swal from 'sweetalert2';
import { apiUrl } from '../../config';
import ModalContrasena from './ModalContrasena';

const Perfil = () => {
    const [loading, setLoading] = useState(false);
    const [id, setId] = useState('');
    const [nombre, setNombre] = useState('');
    const [apellido, setApellido] = useState('');
    const [correo, setCorreo] = useState('');

    const token = localStorage.getItem('token');
    const modalContrasenaRef = useRef(null);

    const obtenerEstudiante = async () => {
        setLoading(true);

        try {
            const response = await fetch(`${apiUrl}/api/Estudiantes/obtenerEstudianteActual`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!response.ok) {
                throw new Error(await response.text());
            }

            const data = await response.json();
            setId(data.id);
            setNombre(data.nombre);
            setApellido(data.apellido);
            setCorreo(data.correO_ELECTRONICO);

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

    const actualizarEstudiante = async (e) => {
        e.preventDefault();
        setLoading(true);

        // Objeto para enviar al backend
        const estudiante = {
            ID: id,
            NOMBRE: nombre,
            APELLIDO: apellido
        };

        try {
            const response = await fetch(`${apiUrl}/api/Estudiantes/actualizarEstudiante`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(estudiante)
            });

            if (response.ok) {
                Swal.fire({
                    title: '¡Éxito!',
                    text: 'Estudiante actualizado con éxito.',
                    icon: 'success',
                    confirmButtonText: 'Aceptar'
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

    const mostrarModal = () => {
        modalContrasenaRef.current.limpiarCampos(); // Limpiar los campos antes de mostrar el modal
        const modal = new bootstrap.Modal(document.getElementById('modalContrasena'));
        modal.show();
    };

    useEffect(() => {
        obtenerEstudiante();
    }, []);

    return (
        <Navbar>
            <div className="container-fluid">
                <div className="d-sm-flex align-items-center justify-content-between mb-4">
                    <h1 className="h3 mb-0 text-gray-800">Actualizar Información</h1>
                </div>

                <div className="card">
                    <div className="card-body">
                        <form onSubmit={actualizarEstudiante}>
                            <div className="form-group row">
                                <div className="col-sm-6 mb-3 mb-sm-0">
                                    <input type="text" className="form-control form-control-user" id="nombre" placeholder="Nombres" value={nombre} onChange={(e) => setNombre(e.target.value)}/>
                                </div>
                                <div className="col-sm-6">
                                    <input type="text" className="form-control form-control-user" id="apellido" placeholder="Apellidos" value={apellido} onChange={(e) => setApellido(e.target.value)}/>
                                </div>
                            </div>
                            <div className="form-group row">
                                <div className="col-sm-6 mb-3 mb-sm-0">
                                    <input type="text" className="form-control form-control-user" id="correo" placeholder="Correo Electrónico" value={correo} disabled/>
                                </div>
                                <div className="col-sm-6">
                                    <button className="btn btn-success"><i className="fas fa-save"></i></button>
                                </div>
                            </div>
                        </form>

                        <hr/>

                        <div className="form-group row">
                            <div className="col-sm-6">
                                <button className="btn btn-primary" onClick={mostrarModal}>Actualizar Contraseña</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {loading && (
                <div className="overlay">
                    <div className="spinner-container">
                        <ReactLoading type="spin" color="#007bff" height={100} width={100} />
                    </div>
                </div>
            )}

            <ModalContrasena ref={modalContrasenaRef} />
        </Navbar>
    );
};

export default Perfil;
