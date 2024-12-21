import React, { useEffect, useState } from 'react';
import Swal from 'sweetalert2';
import { apiUrl } from '../../config';
import ReactPaginate from 'react-paginate';


const ModalMateria = ({ materia }) => {
    const [estudiantes, setEstudiantes] = useState([]);
    const [loading, setLoading] = useState(false);
    const [paginaActual, setPaginaActual] = useState(0);

    const token = localStorage.getItem('token');

    const obtenerEstudiantesXMateria = async () => {
        setLoading(true);

        try {
            const response = await fetch(`${apiUrl}/api/MateriaEstudiante/obtenerEstudiantesXMateria?materiaId=${materia.materiA_ID}`, {
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
            setEstudiantes(data);

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
        if (materia && materia.materiA_ID) {
            setPaginaActual(0);
            obtenerEstudiantesXMateria();
        }
    }, [materia]);

    const estudiantesXPagina = 5;

    // Calcular el rango de estudiantes para la página actual
    const estudiantesOmitir = paginaActual * estudiantesXPagina;
    const currentStudents = estudiantes.slice(estudiantesOmitir, estudiantesOmitir + estudiantesXPagina);

    // Función para manejar el cambio de página
    const cambiarPagina = (data) => {
        setPaginaActual(data.selected);
    };

    return (
        <div className="modal fade" id="modalMateria" tabIndex="-1" aria-labelledby="modalMateriaLabel" aria-hidden="true">
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title" id="modalMateriaLabel">
                            {materia ? materia.materiA_NOMBRE + ' - Compañeros de clase' : 'Detalle de la Materia'}
                        </h5>
                        <button type="button" className="btn btn-sm" data-dismiss="modal" aria-label="Close">X</button>
                    </div>

                    <div className="modal-body">
                        <div className="table-responsive">
                            <table id="estudiantesTable" className="table table-striped">
                                <thead>
                                    <tr>
                                        <th>Nombre</th>
                                        <th>Apellido</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {currentStudents.length > 0 ? (
                                        currentStudents.map((estudiante) => (
                                            <tr key={estudiante.estudiantE_ID}>
                                                <td>{estudiante.estudiantE_NOMBRE}</td>
                                                <td>{estudiante.estudiantE_APELLIDO}</td>
                                            </tr>
                                        ))
                                    ) : (
                                        <tr>
                                            <td colSpan="2">No hay estudiantes disponibles</td>
                                        </tr>
                                    )}
                                </tbody>
                            </table>
                        </div>

                        <div className="d-flex justify-content-center">
                            <ReactPaginate
                                previousLabel={"Anterior"}
                                nextLabel={"Siguiente"}
                                breakLabel={"..."}
                                pageCount={Math.ceil(estudiantes.length / estudiantesXPagina)}
                                marginPagesDisplayed={2}
                                pageRangeDisplayed={5}
                                onPageChange={cambiarPagina}
                                containerClassName={"pagination pagination-sm"}
                                pageClassName={"page-item"}
                                pageLinkClassName={"page-link"}
                                activeClassName={"active"}
                                disabledClassName={"disabled"}
                                previousClassName={"page-item"}
                                nextClassName={"page-item"}
                                previousLinkClassName={"page-link"}
                                nextLinkClassName={"page-link"}   
                                forcePage={paginaActual}
                            />
                        </div>
                    </div>

                    <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ModalMateria;
