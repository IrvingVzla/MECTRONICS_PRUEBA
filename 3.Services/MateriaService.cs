using MECTRONICS._1.Database;
using MECTRONICS._2.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace MECTRONICS._3.Services
{
    public class MateriaService
    {
        private readonly ApplicationDbContext _context;

        public MateriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VMateria>> ObtenerMateriasAsync()
        {
            return await _context.VMaterias.ToListAsync();
        }

    }
}
