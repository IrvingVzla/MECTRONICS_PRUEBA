using MECTRONICS._1.Database;
using MECTRONICS._2.Models;
using Microsoft.EntityFrameworkCore;

namespace MECTRONICS._3.Services
{
    public class MateriaService
    {
        private readonly ApplicationDbContext _context;

        public MateriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para obtener todas las materias
        /// <summary>
        /// Método que obtiene una lista de todas las materias disponibles en la base de datos.
        /// </summary>
        /// <returns>Lista de objetos VMateria que representan las materias.</returns>
        public async Task<List<VMateria>> ObtenerMateriasAsync()
        {
            return await _context.VMaterias.ToListAsync();
        }
    }
}
