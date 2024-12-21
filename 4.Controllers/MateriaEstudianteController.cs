using MECTRONICS._2.Models;
using MECTRONICS._3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MECTRONICS._4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaEstudianteController : ControllerBase
    {
        private readonly MateriaEstudianteService _materiaEstudianteService;

        public MateriaEstudianteController(MateriaEstudianteService materiaEstudianteService)
        {
            _materiaEstudianteService = materiaEstudianteService;
        }

        [Authorize]
        [HttpGet("obtenerMateriasXEstudiante")]
        /// <summary>
        /// Metodo que obtiene las materias asignadas a un estudiante autenticado.
        /// </summary>
        /// <returns>Lista de materias del estudiante o mensaje de error.</returns>
        public async Task<ActionResult<List<VMateriaEstudiante>>> MateriasXEstudiante()
        {
            try
            {
                var correo = User.Identity.Name;
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var materiasEstudiante = await _materiaEstudianteService.ObtenerMateriasXEstudianteAsync(userId);

                return Ok(materiasEstudiante);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("agregarMateriaXEstudiante")]
        /// <summary>
        /// Metodo que agrega una nueva materia a un estudiante.
        /// </summary>
        /// <param name="materiaEstudiante">Objeto que contiene los datos de la materia a agregar.</param>
        /// <returns>Mensaje de exito si se agrega correctamente o mensaje de error si no es asi.</returns>
        public async Task<ActionResult> AgregarMateria([FromBody] MateriaEstudiante materiaEstudiante)
        {
            try
            {
                if (materiaEstudiante == null)
                {
                    return BadRequest("No hay datos para procesar la solicitud.");
                }

                if (materiaEstudiante.MATERIA_ID == 0)
                {
                    return BadRequest("No hay campos para procesar la solicitud.");
                }

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                materiaEstudiante.ESTUDIANTE_ID = userId;
                await _materiaEstudianteService.AgregarMateriaXEstudianteAsync(materiaEstudiante);

                return Ok("Materia agregada al estudiante.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("quitarMateriaXEstudiante")]
        /// <summary>
        /// Metodo que elimina una materia de un estudiante.
        /// </summary>
        /// <param name="materiaEstudiante">Objeto que contiene los datos de la materia a eliminar.</param>
        /// <returns>Mensaje de exito si se elimina correctamente o mensaje de error si no es asi.</returns>
        public async Task<ActionResult> QuitarMateria([FromBody] MateriaEstudiante materiaEstudiante)
        {
            try
            {
                if (materiaEstudiante == null)
                {
                    return BadRequest("No hay datos para procesar la solicitud.");
                }

                if (materiaEstudiante.MATERIA_ID == 0)
                {
                    return BadRequest("No hay campos para procesar la solicitud.");
                }

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                materiaEstudiante.ESTUDIANTE_ID = userId;
                await _materiaEstudianteService.EliminarMateriaXEstudianteAsync(materiaEstudiante);

                return Ok("Materia eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("obtenerEstudiantesXMateria")]
        /// <summary>
        /// Metodo que obtiene los estudiantes asignados a una materia.
        /// </summary>
        /// <param name="materiaId">Id de la materia para obtener los estudiantes asociados.</param>
        /// <returns>Lista de estudiantes o mensaje de error si no se encuentra la materia.</returns>
        public async Task<ActionResult<List<VMateriaEstudiante>>> EstudiantesXMateria([FromQuery] int materiaId)
        {
            try
            {
                if (materiaId == 0)
                {
                    return BadRequest("No se enviaron parametros para la solicitud.");
                }

                var estudiantesXMateria = await _materiaEstudianteService.ObtenerEstudiantesXMateriaAsync(materiaId);

                return Ok(estudiantesXMateria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
