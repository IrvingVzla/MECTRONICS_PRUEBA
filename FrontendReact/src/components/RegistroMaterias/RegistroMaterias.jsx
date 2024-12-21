import React, { useEffect, useState } from 'react';
import Navbar from '../Navbar';
import ReactLoading from 'react-loading';
import Swal from 'sweetalert2';
import { apiUrl } from '../../config';
import './RegistroMaterias.css';

const RegistroMaterias = () => {
    const [materias, setMaterias] = useState([]);
    const [materiasEstudiante, setMateriasEstudiante] = useState([]); 
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const token = localStorage.getItem('token');

    // Función para obtener las materias
    const obtenerMaterias = async () => {
        setLoading(true);

        try {
            const response = await fetch(`${apiUrl}/api/Materias/obtenerMaterias`, {
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
            setMaterias(data); // Guardamos las materias en el estado

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
            setMateriasEstudiante(data); // Guardamos las materias asignadas

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

    // Función para agregar una materia al estudiante
    const agregarMateria = async (materiaId) => {
        setLoading(true);

        try {
            const response = await fetch(`${apiUrl}/api/MateriaEstudiante/agregarMateriaXEstudiante`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({
                    MATERIA_ID: materiaId })
            });

            if (response.ok) {
                Swal.fire({
                    title: '¡Éxito!',
                    text: 'Materia agregada al estudiante.',
                    icon: 'success',
                    confirmButtonText: 'Aceptar'
                });

                obtenerMateriasEstudiante();
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

    // Función para quitar una materia del estudiante
    const quitarMateria = async (materiaId) => {
        setLoading(true);

        try {
            const response = await fetch(`${apiUrl}/api/MateriaEstudiante/quitarMateriaXEstudiante`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({
                    MATERIA_ID: materiaId
                })
            });

            if (response.ok) {
                Swal.fire({
                    title: '¡Éxito!',
                    text: 'Materia eliminada correctamente.',
                    icon: 'success',
                    confirmButtonText: 'Aceptar'
                });

                obtenerMateriasEstudiante();
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

    useEffect(() => {
        obtenerMaterias();
        obtenerMateriasEstudiante();
    }, []);

    useEffect(() => {
        if (materias.length > 0) {
            const table = $('#materiasTable').DataTable({
                destroy: true, // Destruye la tabla anterior
                pageLength: 10,
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json',
                },
                columnDefs: [
                    {
                        targets: -1, // Última columna (Acciones)
                        orderable: false // Desactiva la ordenación
                    }
                ]
            });

            return () => {
                if ($.fn.DataTable.isDataTable('#materiasTable')) {
                    table.destroy();
                }
            };
        }
    }, [materias]);


    return (
        <Navbar>
            <div className="container-fluid">
                <div className="d-sm-flex align-items-center justify-content-between mb-4">
                    <h1 className="h3 mb-0 text-gray-800">Registro de materias</h1>
                </div>

                {error && <p className="text-danger">{error}</p>}

                {loading && (
                    <div className="overlay">
                        <div className="spinner-container">
                            <ReactLoading type="spin" color="#007bff" height={100} width={100} />
                        </div>
                    </div>
                )}

                <div className="card">
                    <div className="card-body">
                        <div className="table-responsive">
                            <table id="materiasTable" className="table table-striped">
                                <thead>
                                    <tr>
                                        <th>Nombre</th>
                                        <th>Docente</th>
                                        <th>Créditos</th>
                                        <th>Acción</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {materias.map((materia) => (
                                        <tr key={materia.materiA_ID}>
                                            <td>{materia.materiA_NOMBRE}</td>
                                            <td>{materia.profesoR_NOMBRE && materia.profesoR_APELLIDO ? `${materia.profesoR_NOMBRE} ${materia.profesoR_APELLIDO}` : 'No asignado'}</td>
                                            <td>{materia.creditos}</td>
                                            <td>
                                                {materiasEstudiante.some(m => m.materiA_ID === materia.materiA_ID) ?
                                                (
                                                        <button className="btn btn-danger" onClick={() => quitarMateria(materia.materiA_ID)}> <i className="fas fa-trash"></i></button>
                                                ) :
                                                (
                                                        <button className="btn btn-success" onClick={() => agregarMateria(materia.materiA_ID)}> <i className="fas fa-plus"></i> </button>
                                                )}
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </Navbar>
    );
};

export default RegistroMaterias;
