import React, {useState, useRef, forwardRef, useImperativeHandle } from 'react';
import ReactLoading from 'react-loading';
import Swal from 'sweetalert2';
import { apiUrl } from '../../config';

const ModalContrasena = forwardRef((props, ref) => {
    const [contrasenaActual, setContrasenaActual] = useState('');
    const [contrasenaNueva, setContrasenaNueva] = useState('');
    const [confirmarContrasena, setConfirmarContrasena] = useState('');
    const [loading, setLoading] = useState(false);

    const token = localStorage.getItem('token');
    const cerrarModalRef = useRef(null);

    // Función para limpiar los campos
    const limpiarCampos = () => {
        setContrasenaActual('');
        setContrasenaNueva('');
        setConfirmarContrasena('');
    };

    // Exponer la función limpiarCampos
    useImperativeHandle(ref, () => ({
        limpiarCampos,
    }));

    // Función para manejar el cambio de contraseña
    const cambiarContrasena = async () => {
        if (contrasenaNueva !== confirmarContrasena) {
            Swal.fire({
                title: 'Error',
                text: 'Las contraseñas no coinciden',
                icon: 'error',
                confirmButtonText: 'Intentar de nuevo'
            });

            return;
        }
        if (contrasenaActual == contrasenaNueva) {
            Swal.fire({
                title: 'Error',
                text: 'La nueva contraseña no puede ser la misma que la actual.',
                icon: 'error',
                confirmButtonText: 'Intentar de nuevo'
            });

            return;
        }
        if (!contrasenaActual || !contrasenaNueva || !confirmarContrasena) {
            Swal.fire({
                title: 'Error',
                text: 'Todos los campos son obligatorios',
                icon: 'error',
                confirmButtonText: 'Intentar de nuevo'
            });
            
            return;
        }

        setLoading(true);

        try {
            const response = await fetch(`${apiUrl}/api/Login/actualizarContrasena`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`,
                },
                body: JSON.stringify({
                    CONTRASENA_ACTUAL: contrasenaActual,
                    CONTRASENA_NUEVA: contrasenaNueva,
                }),
            });

            if (response.ok) {
                Swal.fire({
                    title: '¡Éxito!',
                    text: 'Contraseña actualizada con éxito.',
                    icon: 'success',
                    confirmButtonText: 'Aceptar'
                });

                cerrarModalRef.current.click();

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
        <div className="modal fade" id="modalContrasena" tabIndex="-1" aria-labelledby="modalContrasenaLabel" aria-hidden="true">
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title" id="modalContrasenaLabel">Actualizar Contraseña</h5>
                        <button type="button" className="btn btn-sm" data-dismiss="modal" aria-label="Close">X</button>
                    </div>

                    <div className="modal-body">
                        <div className="form-group row">
                            <div className="col-sm-6 mb-3 mb-sm-0">
                                <input type="password" className="form-control form-control-user" id="contrasenaActual" placeholder="Contraseña Actual" value={contrasenaActual} onChange={(e) => setContrasenaActual(e.target.value)}/>
                            </div>
                        </div>

                        <div className="form-group row">
                            <div className="col-sm-6">
                                <input type="password" className="form-control form-control-user" id="contrasenaNueva" placeholder="Contraseña Nueva" value={contrasenaNueva} onChange={(e) => setContrasenaNueva(e.target.value)} />
                            </div>
                            <div className="col-sm-6 mb-3 mb-sm-0">
                                <input type="password" className="form-control form-control-user" id="confirmarContrasena" placeholder="Confirmar Contraseña" value={confirmarContrasena} onChange={(e) => setConfirmarContrasena(e.target.value)} />
                            </div>
                        </div>

                        <div className="form-group row">
                            <div className="col-sm-12">
                                <button className="btn btn-success col-sm-12" onClick={cambiarContrasena} disabled={loading}><i className="fas fa-save"></i></button>
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

                    <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" ref={cerrarModalRef} data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    );
});

export default ModalContrasena;
