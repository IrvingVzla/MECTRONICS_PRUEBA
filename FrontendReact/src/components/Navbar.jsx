import React, { useState, useEffect } from 'react';
import { Link, Navigate } from 'react-router-dom';

const Navbar = ({ children }) => { // children inserta el contenido en la pagina.
    const [redirect, setRedirect] = useState(false);
    const nombre = localStorage.getItem('nombre');

    const cerrarSesion = () => {
        localStorage.clear();
        setRedirect(true);
    };

    if (redirect) {
        return <Navigate to="/" replace />;
    }

    return (
        <div id="wrapper" className="d-flex">
            {/* Sidebar */}
            <ul className="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">
                <Link className="sidebar-brand d-flex align-items-center justify-content-center" to="/PaginaPrincipal">
                    <div className="sidebar-brand-icon rotate-n-15">
                        <i className="fas fa-laugh-wink"></i>
                    </div>
                    <div className="sidebar-brand-text mx-3">Mectronics</div>
                </Link>

                <hr className="sidebar-divider my-0" />
                <hr className="sidebar-divider" />

                <div className="sidebar-heading">
                    Navegación
                </div>

                <li className="nav-item">
                    <Link className="nav-link" to="/PaginaPrincipal">
                        <i className="fas fa-fw fa-home"></i>
                        <span>Inicio</span>
                    </Link>
                </li>

                <li className="nav-item">
                    <Link className="nav-link" to="/RegistroMaterias">
                        <i className="fas fa-fw fa-book"></i>
                        <span>Registro de Materias</span>
                    </Link>
                </li>

                <hr className="sidebar-divider d-none d-md-block" />
            </ul>
            {/* Final del Sidebar */}

            <div id="content-wrapper" className="d-flex flex-column w-100">
                <div id="content">
                    <nav className="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">
                        <ul className="navbar-nav ml-auto">
                            <li className="nav-item dropdown no-arrow mx-1">
                                <a className="nav-link dropdown-toggle" href="#" id="alertsDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i className="fas fa-bell fa-fw"></i>
                                    <span className="badge badge-danger badge-counter">3+</span>
                                </a>
                            </li>

                            <li className="nav-item dropdown no-arrow mx-1">
                                <a className="nav-link dropdown-toggle" href="#" id="messagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i className="fas fa-envelope fa-fw"></i>
                                    <span className="badge badge-danger badge-counter">7</span>
                                </a>
                            </li>

                            <div className="topbar-divider d-none d-sm-block"></div>

                            <li className="nav-item dropdown no-arrow">
                                <a className="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <span className="mr-2 d-none d-lg-inline text-gray-600 small">{nombre}</span>
                                    <div className="img-profile rounded-circle"><i className="far fa-user-circle" style={{ fontSize: 32 }}></i></div>
                                </a>
                                <div className="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="userDropdown">
                                    <Link className="dropdown-item" to="/Perfil">
                                        <i className="fas fa-user fa-sm fa-fw mr-2 text-gray-400"></i>
                                        Perfil
                                    </Link>
                                    <div className="dropdown-divider"></div>
                                    <button className="dropdown-item" onClick={cerrarSesion}>
                                        <i className="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i>
                                        Cerrar Sesión
                                    </button>
                                </div>
                            </li>
                        </ul>
                    </nav>

                    {/* Aqui se muestar el contenido de las paginas*/}
                    <div className="container-fluid">
                        {children}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Navbar;
