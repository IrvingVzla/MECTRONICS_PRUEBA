using MECTRONICS._2.Models;
using MECTRONICS._3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MECTRONICS._4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LoginService _loginService;

        public LoginController(IConfiguration configuration, LoginService loginService)
        {
            _configuration = configuration;
            _loginService = loginService;
        }

        [HttpPost("iniciarSesion")]
        /// <summary>
        /// Metodo que permite iniciar sesion validando las credenciales del usuario.
        /// </summary>
        /// <param name="login">Objeto Login que contiene el correo electronico y la contrasena.</param>
        /// <returns>Token JWT si las credenciales son correctas o mensaje de error si no lo son.</returns>
        public async Task<IActionResult> IniciarSesion([FromBody] Login login)
        {
            try
            {
                if (login == null)
                {
                    return BadRequest("Debe añadir datos del login.");
                }

                if (string.IsNullOrWhiteSpace(login.CORREO_ELECTRONICO) || string.IsNullOrWhiteSpace(login.CONTRASENA))
                {
                    return BadRequest("Todos los campos son obligatorios.");
                }

                bool esCorrecto = await _loginService.ValidarCredenciales(login);

                if (esCorrecto)
                {
                    var estudiante = await _loginService.ObtenerInfoEstudiante(login.CORREO_ELECTRONICO);

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, login.CORREO_ELECTRONICO),
                        new Claim(ClaimTypes.NameIdentifier, estudiante.ID.ToString()),
                        new Claim(ClaimTypes.GivenName, estudiante.NOMBRE),
                        new Claim(ClaimTypes.Surname, estudiante.APELLIDO)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: creds
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(new { Token = tokenString, Nombre = estudiante.NOMBRE });
                }

                return Unauthorized("Las credenciales son incorrectas.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Ruta que valida si el token es válido
        [HttpGet("validarToken")]
        /// <summary>
        /// Metodo que valida si el token proporcionado es valido y no ha expirado.
        /// </summary>
        /// <returns>Respuesta con estado de validez del token.</returns>
        public IActionResult ValidarToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token no proporcionado.");
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken == null)
                {
                    return Unauthorized("Token inválido.");
                }

                // Verifica si el token ha expirado
                if (jsonToken.ValidTo < DateTime.UtcNow)
                {
                    return Unauthorized("Token expirado.");
                }

                var correo = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(correo))
                {
                    return Unauthorized("Token inválido o expirado.");
                }

                return Ok(new { valid = true });
            }
            catch (Exception ex)
            {
                return Unauthorized("Error al validar el token.");
            }
        }

        [Authorize]
        [HttpPut("actualizarContrasena")]
        /// <summary>
        /// Metodo que permite actualizar la contrasena de un usuario autenticado.
        /// </summary>
        /// <param name="actualizarContrasena">Objeto con las contrasenas actual y nueva.</param>
        /// <returns>Mensaje de exito si se actualiza correctamente o mensaje de error si no es asi.</returns>
        public async Task<ActionResult> actualizarContrasena([FromBody] ActualizarContrasena actualizarContrasena)
        {
            try
            {
                if (actualizarContrasena.CONTRASENA_ACTUAL == actualizarContrasena.CONTRASENA_NUEVA)
                {
                    return BadRequest("La nueva contraseña no puede ser la misma que la actual.");
                }

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var correo = User.Identity.Name;
                await _loginService.ActualizarContrasenaAsync(actualizarContrasena, correo, userId);

                return Ok("Se ha actualizado el estudiante con éxito.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
