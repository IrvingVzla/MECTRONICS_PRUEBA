using MECTRONICS._2.Models;
using MECTRONICS._3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MECTRONICS._4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly EstudianteService _estudianteService;

        public EstudiantesController(EstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

        [Authorize]
        [HttpGet("obtenerEstudiantes")]
        public async Task<ActionResult<List<Estudiante>>> ObtenerEstudiantes()
        {
            var estudiantes = await _estudianteService.ObtenerEstudiantesAsync();
            if (estudiantes == null || estudiantes.Count == 0)
            {
                return NotFound("No se encontraron estudiantes.");
            }

            return Ok(estudiantes);
        }

        [HttpPost("crearEstudiante")]
        public async Task<ActionResult> CrearEstudiante([FromBody] Estudiante estudiante)
        {
            try
            {

                if (estudiante == null)
                {
                    return BadRequest("Debe añadir datos del estudiante.");
                }

                if (string.IsNullOrWhiteSpace(estudiante.NOMBRE) || string.IsNullOrWhiteSpace(estudiante.APELLIDO) || string.IsNullOrWhiteSpace(estudiante.CORREO_ELECTRONICO))
                {
                    return BadRequest("Todos los campos son obligatorios.");
                }

                // Verificar si el correo electrónico ya está en uso
                var estudianteExistente = await _estudianteService.ObtenerEstudiantesAsync();
                if (estudianteExistente.Any(e => e.CORREO_ELECTRONICO == estudiante.CORREO_ELECTRONICO))
                {
                    return BadRequest("El correo electrónico ya está registrado.");
                }

                DateTime fechaActual = DateTime.Now;
                estudiante.FECHA_REGISTRO = fechaActual;

                // Agregar el estudiante
                await _estudianteService.AgregarEstudianteAsync(estudiante);

                return Ok("Se ha creado el estudiante con éxito.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("obtenerEstudianteActual")]
        public async Task<ActionResult<Estudiante>> EstudianteActual()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var estudiante = await _estudianteService.ObtenerEstudianteActualXIDAsync(userId);

            if (estudiante == null)
            {
                return NotFound("No se encontró informacion.");
            }

            return Ok(estudiante);
        }

        [Authorize]
        [HttpPut("actualizarEstudiante")]

        public async Task<ActionResult> actualizarEstudiante([FromBody] Estudiante estudiante)
        {
            try
            {
                if (estudiante == null)
                {
                    return BadRequest("No se han añadido datos del usuario..");
                }

                await _estudianteService.ActualizarEstudianteAsync(estudiante);

                return Ok("Se ha actualizado el estudiante con éxito.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
