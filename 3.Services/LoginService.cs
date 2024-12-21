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

        public async Task<string> ObtenerContrasenaXCorreoAsync(string correo)
        {
            var contrasenaEncrypt = await _context.Estudiantes
                .Where(u => u.CORREO_ELECTRONICO == correo)
                .Select(u => u.CONTRASENA)
                .FirstOrDefaultAsync();

            return contrasenaEncrypt;
        }

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

        // Método para encriptar la contraseña
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

        // Método para desencriptar la contraseña
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
        public async Task<Estudiante> ObtenerInfoEstudiante(string correo)
        {
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.CORREO_ELECTRONICO == correo);

            if (estudiante == null)
            {
                throw new Exception("No se ha encontrado el estudiante.");
            }

            return estudiante;
        }

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
