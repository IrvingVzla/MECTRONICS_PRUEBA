using MECTRONICS._1.Database;
using MECTRONICS._2.Models;
using Microsoft.EntityFrameworkCore;

namespace MECTRONICS._3.Services
{
    public class MateriaEstudianteService
    {
        private readonly ApplicationDbContext _context;

        public MateriaEstudianteService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para obtener las materias asignadas a un estudiante específico
        /// <summary>
        /// Método que obtiene una lista de las materias asignadas a un estudiante.
        /// </summary>
        /// <param name="estudianteId">ID del estudiante para el cual se desean obtener las materias.</param>
        /// <returns>Lista de materias asignadas al estudiante.</returns>
        public async Task<List<VMateriaEstudiante>> ObtenerMateriasXEstudianteAsync(int estudianteId)
        {
            return await _context.VMaterias_Estudiantes
                .Where(me => me.ESTUDIANTE_ID == estudianteId)
                .ToListAsync();
        }

        // Método para agregar una materia a un estudiante
        /// <summary>
        /// Método que agrega una materia a un estudiante después de realizar varias validaciones.
        /// </summary>
        /// <param name="materiaEstudiante">Objeto que representa la relación entre el estudiante y la materia.</param>
        /// <exception cref="Exception">Lanza excepciones si no se cumplen ciertas condiciones.</exception>
        public async Task AgregarMateriaXEstudianteAsync(MateriaEstudiante materiaEstudiante)
        {
            // Verificar que la materia tenga un docente asignado
            var materia = await _context.VMaterias
                .FirstOrDefaultAsync(m => m.MATERIA_ID == materiaEstudiante.MATERIA_ID);

            if (materia == null || materia.PROFESOR_ID == null)
            {
                throw new Exception("La materia debe tener un docente asignado.");
            }

            // Verificar que el estudiante no tenga más de una materia con el mismo docente
            var existeMateriaConMismoDocente = await _context.VMaterias_Estudiantes
                .AnyAsync(me => me.ESTUDIANTE_ID == materiaEstudiante.ESTUDIANTE_ID &&
                                me.PROFESOR_ID == materia.PROFESOR_ID);

            if (existeMateriaConMismoDocente)
            {
                throw new Exception("El estudiante ya está inscrito en una materia con este docente.");
            }

            // Verificar que el estudiante no tenga más de 3 materias
            var materiasEstudiante = await ObtenerMateriasXEstudianteAsync(materiaEstudiante.ESTUDIANTE_ID);
            if (materiasEstudiante.Count >= 3)
            {
                throw new Exception("Solo puede tener 3 materias como máximo.");
            }

            // Agregar la materia al estudiante
            _context.Materias_Estudiantes.Add(materiaEstudiante);
            await _context.SaveChangesAsync();
        }

        // Método para eliminar la relación de materia asignada a un estudiante
        /// <summary>
        /// Método que elimina la relación entre un estudiante y una materia específica.
        /// </summary>
        /// <param name="materiaEstudiante">Objeto que representa la relación entre el estudiante y la materia.</param>
        /// <exception cref="Exception">Lanza una excepción si no se encuentra la relación para eliminar.</exception>
        public async Task EliminarMateriaXEstudianteAsync(MateriaEstudiante materiaEstudiante)
        {
            var relacion = await _context.Materias_Estudiantes
                .FirstOrDefaultAsync(me => me.ESTUDIANTE_ID == materiaEstudiante.ESTUDIANTE_ID && me.MATERIA_ID == materiaEstudiante.MATERIA_ID);

            if (relacion == null)
            {
                throw new Exception("No existe el elemento a eliminar.");
            }

            // Eliminar la relación de la materia con el estudiante
            _context.Materias_Estudiantes.Remove(relacion);
            await _context.SaveChangesAsync();
        }

        // Método para obtener los estudiantes asignados a una materia específica
        /// <summary>
        /// Método que obtiene una lista de estudiantes que están inscritos en una materia.
        /// </summary>
        /// <param name="materiaId">ID de la materia para la cual se desean obtener los estudiantes.</param>
        /// <returns>Lista de estudiantes inscritos en la materia.</returns>
        public async Task<List<VMateriaEstudiante>> ObtenerEstudiantesXMateriaAsync(int materiaId)
        {
            return await _context.VMaterias_Estudiantes
                .Where(me => me.MATERIA_ID == materiaId)
                .ToListAsync();
        }
    }
}
