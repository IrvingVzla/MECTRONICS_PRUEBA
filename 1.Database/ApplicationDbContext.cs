using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MECTRONICS._2.Models;

namespace MECTRONICS._1.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<MateriaEstudiante> Materias_Estudiantes { get; set; }
        public DbSet<VMateriaEstudiante> VMaterias_Estudiantes { get; set; }
        public DbSet<VMateria> VMaterias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar la vistas sin claves primarias
            modelBuilder.Entity<VMateriaEstudiante>(entity =>
            {
                entity.HasNoKey();  // Indica que esta entidad no tiene clave primaria
                entity.ToView("VMATERIAS_ESTUDIANTES");  // Especifica la vista en la base de datos
            });

            modelBuilder.Entity<VMateria>(entity =>
            {
                entity.HasNoKey();  // Indica que esta entidad no tiene clave primaria
                entity.ToView("VMATERIAS");  // Especifica la vista en la base de datos
            });
        }
    }
}
