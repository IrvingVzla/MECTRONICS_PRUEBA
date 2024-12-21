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
