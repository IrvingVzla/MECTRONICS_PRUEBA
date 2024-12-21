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
        /// <summary>
        /// Metodo que obtiene todos los estudiantes registrados en la base de datos.
        /// </summary>
        /// <returns>Lista de estudiantes si se encuentran, o un mensaje de no encontrados si no hay estudiantes.</returns>
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
        /// <summary>
        /// Metodo que se encarga de crear estudiantes, realiza validaciones para que el objeto no llegue nulo y verificar que el correo no este registrado.
        /// </summary>
        /// <param name="estudiante">Objeto Estudiante con la informacion a registrar.</param>
        /// <returns>Mensaje de exito si el estudiante se crea correctamente o mensaje de error en caso contrario.</returns>
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
        /// <summary>
        /// Metodo que obtiene el estudiante actual basado en el ID del usuario autenticado.
        /// </summary>
        /// <returns>El estudiante actual o un mensaje de no encontrado si no existe.</returns>
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
        /// <summary>
        /// Metodo que actualiza la informacion de un estudiante.
        /// </summary>
        /// <param name="estudiante">Objeto Estudiante con los datos actualizados.</param>
        /// <returns>Mensaje de exito si se actualiza correctamente o mensaje de error en caso contrario.</returns>
        public async Task<ActionResult> actualizarEstudiante([FromBody] Estudiante estudiante)
        {
            try
            {
                if (estudiante == null)
                {
                    return BadRequest("No se han añadido datos del usuario.");
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
