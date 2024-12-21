using MECTRONICS._1.Database;
using MECTRONICS._2.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace MECTRONICS._3.Services
{
    public class MateriaEstudianteService
    {
        private readonly ApplicationDbContext _context;

        public MateriaEstudianteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VMateriaEstudiante>> ObtenerMateriasXEstudianteAsync(int estudianteId)
        {
            return await _context.VMaterias_Estudiantes
                .Where(me => me.ESTUDIANTE_ID == estudianteId)
                .ToListAsync();
        }

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

            // Agregar la materia
            _context.Materias_Estudiantes.Add(materiaEstudiante);
            await _context.SaveChangesAsync();
        }


        public async Task EliminarMateriaXEstudianteAsync(MateriaEstudiante materiaEstudiante)
        {
            var relacion = await _context.Materias_Estudiantes
                .FirstOrDefaultAsync(me => me.ESTUDIANTE_ID == materiaEstudiante.ESTUDIANTE_ID && me.MATERIA_ID == materiaEstudiante.MATERIA_ID);

            if (relacion == null)
            {
                throw new Exception("No existe el elemento a eliminar.");
            }

            // Eliminar la relación
            _context.Materias_Estudiantes.Remove(relacion);
            await _context.SaveChangesAsync();

        }

        public async Task<List<VMateriaEstudiante>> ObtenerEstudiantesXMateriaAsync(int materiaId)
        {
            return await _context.VMaterias_Estudiantes
                .Where(me => me.MATERIA_ID == materiaId)
                .ToListAsync();
        }
    }
}
