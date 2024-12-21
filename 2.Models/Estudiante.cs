namespace MECTRONICS._2.Models
{
    public class Estudiante
    {
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDO { get; set; }
        public string? CORREO_ELECTRONICO { get; set; }
        public string? CONTRASENA { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }
}
