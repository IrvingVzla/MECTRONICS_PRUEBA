import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import Swal from 'sweetalert2';
import { apiUrl } from '../config';

const PrivateRoute = ({ children }) => {
    const [isTokenValid, setIsTokenValid] = useState(true);
    const token = localStorage.getItem('token');

    useEffect(() => {
        // Verifica si el token es válido
        const validarTokenExpiracion = async () => {
            if (!token) {
                return;
            }

            try {
                const response = await fetch(`${apiUrl}/api/Login/validarToken`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                    },
                });

                if (response.status === 401) {
                    // Si el token ha expirado, muestra un mensaje y elimina el token
                    Swal.fire({
                        title: 'Sesión expirada',
                        text: 'Tu sesión ha caducado, por favor inicia sesión nuevamente.',
                        icon: 'info',
                        confirmButtonText: 'OK',
                    });
                    localStorage.clear();
                    setIsTokenValid(false); // Establece el estado como no válido
                }
            } catch (error) {
                setIsTokenValid(false);
            }
        };

        validarTokenExpiracion();
    });

    // Si no hay token o si el token ha caducado, redirige al login
    if (!token || !isTokenValid) {
        return <Navigate to="/" replace />;
    }

    return children;
};

export default PrivateRoute;
