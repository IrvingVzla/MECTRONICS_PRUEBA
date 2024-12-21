using MECTRONICS._1.Database;
using MECTRONICS._2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MECTRONICS._3.Services
{
    public class LoginService
    {
        private readonly ApplicationDbContext _context;

        private static readonly string key = "ClaveSecreta.d3f";
        private static readonly string iv = "V3ct0r1nic1o1n4d";

        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metodo que obtiene la contrasena encriptada de un estudiante por su correo.
        /// </summary>
        /// <param name="correo">Correo electronico del estudiante.</param>
        /// <returns>Contrasena encriptada.</returns>
        public async Task<string> ObtenerContrasenaXCorreoAsync(string correo)
        {
            var contrasenaEncrypt = await _context.Estudiantes
                .Where(u => u.CORREO_ELECTRONICO == correo)
                .Select(u => u.CONTRASENA)
                .FirstOrDefaultAsync();

            return contrasenaEncrypt;
        }

        /// <summary>
        /// Metodo que valida las credenciales de un estudiante.
        /// </summary>
        /// <param name="login">Objeto Login que contiene correo y contrasena.</param>
        /// <returns>Verdadero si las credenciales son correctas, falso de lo contrario.</returns>
        public async Task<bool> ValidarCredenciales(Login login)
        {
            string contrasenaEncrypt = await ObtenerContrasenaXCorreoAsync(login.CORREO_ELECTRONICO);
            string contrasenaDescryp = DesencriptarContrasena(contrasenaEncrypt);

            if (contrasenaDescryp == login.CONTRASENA)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Metodo para encriptar la contrasena usando AES.
        /// </summary>
        /// <param name="contrasena">Contrasena a encriptar.</param>
        /// <returns>Contrasena encriptada en formato Base64.</returns>
        public string EncriptarContrasena(string contrasena)
        {
            using (Aes aesAlgoritmo = Aes.Create())
            {
                aesAlgoritmo.Key = Encoding.UTF8.GetBytes(key);   // Convertir la clave a bytes
                aesAlgoritmo.IV = Encoding.UTF8.GetBytes(iv);     // Convertir el IV a bytes

                ICryptoTransform encriptor = aesAlgoritmo.CreateEncryptor(aesAlgoritmo.Key, aesAlgoritmo.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encriptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(contrasena);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray()); // Convertir a Base64 para almacenamiento
                }
            }
        }

        /// <summary>
        /// Metodo para desencriptar la contrasena usando AES.
        /// </summary>
        /// <param name="contrasenaEncriptada">Contrasena encriptada en formato Base64.</param>
        /// <returns>Contrasena desencriptada.</returns>
        private string DesencriptarContrasena(string contrasenaEncriptada)
        {
            using (Aes aesAlgoritmo = Aes.Create())
            {
                aesAlgoritmo.Key = Encoding.UTF8.GetBytes(key);   // Convertir la clave a bytes
                aesAlgoritmo.IV = Encoding.UTF8.GetBytes(iv);     // Convertir el IV a bytes

                ICryptoTransform decryptor = aesAlgoritmo.CreateDecryptor(aesAlgoritmo.Key, aesAlgoritmo.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(contrasenaEncriptada)))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Metodo que obtiene la informacion de un estudiante por su correo electronico.
        /// </summary>
        /// <param name="correo">Correo electronico del estudiante.</param>
        /// <returns>El estudiante correspondiente.</returns>
        public async Task<Estudiante> ObtenerInfoEstudiante(string correo)
        {
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.CORREO_ELECTRONICO == correo);

            if (estudiante == null)
            {
                throw new Exception("No se ha encontrado el estudiante.");
            }

            return estudiante;
        }

        /// <summary>
        /// Metodo que actualiza la contrasena de un estudiante.
        /// </summary>
        /// <param name="actualizarContrasena">Objeto con la contrasena actual y la nueva.</param>
        /// <param name="correo">Correo electronico del estudiante.</param>
        /// <param name="userId">ID del estudiante.</param>
        /// <returns>Task</returns>
        public async Task ActualizarContrasenaAsync(ActualizarContrasena actualizarContrasena, string correo, int userId)
        {
            if (string.IsNullOrEmpty(correo) || userId == 0)
            {
                throw new Exception("Ha ocurrido un error al obtener la información del usuario.");
            }

            string contrasenaEncrypt = await ObtenerContrasenaXCorreoAsync(correo);
            string contrasenaDescryp = DesencriptarContrasena(contrasenaEncrypt);

            if (contrasenaDescryp == actualizarContrasena.CONTRASENA_ACTUAL)
            {
                string nuevoEncrypt = EncriptarContrasena(actualizarContrasena.CONTRASENA_NUEVA);

                var estudianteDB = await _context.Estudiantes
                    .Where(me => me.ID == userId)
                    .FirstOrDefaultAsync();

                if (estudianteDB == null)
                {
                    throw new Exception("El estudiante no fue encontrado.");
                }

                estudianteDB.CONTRASENA = nuevoEncrypt;

                _context.Estudiantes.Update(estudianteDB);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("La contraseña actual no es correcta.");
            }
        }
    }
}
