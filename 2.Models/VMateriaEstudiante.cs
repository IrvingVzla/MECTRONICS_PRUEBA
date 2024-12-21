namespace MECTRONICS._2.Models
{
    public class VMateriaEstudiante
    {
        public int ESTUDIANTE_ID { get; set; }
        public int? PROFESOR_ID { get; set; }
        public int MATERIA_ID { get; set; }
        public string MATERIA_NOMBRE { get; set; }
        public string? PROFESOR_NOMBRE { get; set; }
        public string? PROFESOR_APELLIDO { get; set; }
        public string ESTUDIANTE_NOMBRE { get; set; }
        public string ESTUDIANTE_APELLIDO { get; set; }
    }
}
