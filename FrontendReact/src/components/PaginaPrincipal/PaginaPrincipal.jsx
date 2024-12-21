import React, { useEffect, useState } from 'react';
import Navbar from '../Navbar';
import ReactLoading from 'react-loading';
import Swal from 'sweetalert2';
import ModalMateria from './ModalMateria';
import './PaginaPrincipal.css';
import { apiUrl } from '../../config';

const PaginaPrincipal = () => {
    const [materiasEstudiante, setMateriasEstudiante] = useState([]);
    const [loading, setLoading] = useState(false);
    const [materiaSeleccionada, setMateriaSeleccionada] = useState(null);

    const token = localStorage.getItem('token');

    const obtenerMateriasEstudiante = async () => {
        setLoading(true);

        try {
            const response = await fetch(`${apiUrl}/api/MateriaEstudiante/obtenerMateriasXEstudiante`, {
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
            setMateriasEstudiante(data);

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

    const mostrarModal = (materia) => {
        setMateriaSeleccionada(materia);
        const modal = new bootstrap.Modal(document.getElementById('modalMateria'));
        modal.show();
    };

    useEffect(() => {
        obtenerMateriasEstudiante();
    }, []);

    return (
        <Navbar>
            <div className="container-fluid">
                <div className="d-sm-flex align-items-center justify-content-between mb-4">
                    <h1 className="h3 mb-0 text-gray-800">Dashboard</h1>
                </div>

                {loading && (
                    <div className="overlay">
                        <div className="spinner-container">
                            <ReactLoading type="spin" color="#007bff" height={100} width={100} />
                        </div>
                    </div>
                )}

                <div className="row">
                    {materiasEstudiante.length > 0 ? (
                        materiasEstudiante.map((materia, index) => (
                            <div className="col-lg-3 col-md-6 mb-4" key={index} onClick={() => mostrarModal(materia)} style={{ cursor: 'pointer' }}>
                                <div className="card shadow h-100 py-2 hover-shadow hover-scale card-materias">
                                    <div className="card-body">
                                        <h5 className="card-title">{materia.materiA_NOMBRE}</h5>
                                        <p className="card-text">
                                            Profesor: {materia.profesoR_NOMBRE} {materia.profesoR_APELLIDO}
                                        </p>
                                    </div>
                                </div>
                            </div>
                        ))
                    ) : (
                        <p>No tienes materias asignadas.</p>
                    )}
                </div>
            </div>

            <ModalMateria materia={materiaSeleccionada} />
        </Navbar>
    );
};

export default PaginaPrincipal;
