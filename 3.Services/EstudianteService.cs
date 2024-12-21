﻿using MECTRONICS._1.Database;
using MECTRONICS._2.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace MECTRONICS._3.Services
{
    public class EstudianteService
    {
        private readonly ApplicationDbContext _context;
        private readonly LoginService _loginService;

        public EstudianteService(ApplicationDbContext context, LoginService loginService)
        {
            _context = context;
            _loginService = loginService;
        }

        public async Task<List<Estudiante>> ObtenerEstudiantesAsync()
        {
            return await _context.Estudiantes.ToListAsync();
        }

        /// <summary>
        /// Metodo que guarda los estudiantes, encripta la contraseña antes de guardarlo.
        /// </summary>
        /// <param name="estudiante"></param>
        /// <returns></returns>
        public async Task AgregarEstudianteAsync(Estudiante estudiante)
        {
            estudiante.CONTRASENA = _loginService.EncriptarContrasena(estudiante.CONTRASENA);
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
        }

        public async Task<Estudiante> ObtenerEstudianteActualXIDAsync(int userId)
        {
            return await _context.Estudiantes
            .Where(me => me.ID == userId)
            .FirstOrDefaultAsync();
        }

        public async Task ActualizarEstudianteAsync(Estudiante estudiante)
        {
            var estudianteDB = await ObtenerEstudianteActualXIDAsync(estudiante.ID);

            if (estudianteDB == null)
            {
                throw new Exception("El estudiante no fue encontrado.");
            }

            estudianteDB.NOMBRE = estudiante.NOMBRE ?? estudianteDB.NOMBRE;
            estudianteDB.APELLIDO = estudiante.APELLIDO ?? estudianteDB.APELLIDO;

            _context.Estudiantes.Update(estudianteDB);
            await _context.SaveChangesAsync();
        }
    }
}