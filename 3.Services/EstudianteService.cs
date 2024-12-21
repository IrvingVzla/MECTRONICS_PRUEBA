using MECTRONICS._1.Database;
using MECTRONICS._2.Models;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Metodo que obtiene la lista de estudiantes.
        /// </summary>
        /// <returns>Lista de estudiantes.</returns>
        public async Task<List<Estudiante>> ObtenerEstudiantesAsync()
        {
            return await _context.Estudiantes.ToListAsync();
        }

        /// <summary>
        /// Metodo que guarda los estudiantes, encripta la contrasena antes de guardarlo.
        /// </summary>
        /// <param name="estudiante">El estudiante que se va a guardar.</param>
        /// <returns>Task</returns>
        public async Task AgregarEstudianteAsync(Estudiante estudiante)
        {
            estudiante.CONTRASENA = _loginService.EncriptarContrasena(estudiante.CONTRASENA);
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Metodo que obtiene un estudiante por su ID.
        /// </summary>
        /// <param name="userId">ID del estudiante.</param>
        /// <returns>El estudiante encontrado.</returns>
        public async Task<Estudiante> ObtenerEstudianteActualXIDAsync(int userId)
        {
            return await _context.Estudiantes
            .Where(me => me.ID == userId)
            .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Metodo que actualiza los datos de un estudiante.
        /// </summary>
        /// <param name="estudiante">Estudiante con los datos actualizados.</param>
        /// <returns>Task</returns>
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
