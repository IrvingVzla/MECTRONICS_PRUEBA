using MECTRONICS._2.Models;
using MECTRONICS._3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MECTRONICS._4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriasController : ControllerBase
    {
        private readonly MateriaService _materiaService;

        public MateriasController(MateriaService materiaService)
        {
            _materiaService = materiaService;
        }

        [Authorize]
        [HttpGet("obtenerMaterias")]
        /// <summary>
        /// Metodo que obtiene la lista de todas las materias.
        /// </summary>
        /// <returns>Lista de materias o mensaje de error si no se encuentran.</returns>
        public async Task<ActionResult<List<VMateria>>> ObtenerEstudiantes()
        {
            var materias = await _materiaService.ObtenerMateriasAsync();
            if (materias == null || materias.Count == 0)
            {
                return NotFound("No se encontraron materias.");
            }

            return Ok(materias);
        }
    }
}
