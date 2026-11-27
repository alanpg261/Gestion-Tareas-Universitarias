using GestionTareasUniversitarias.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionTareasUniversitarias.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Materia> Materias { get; set; }

    public DbSet<Tarea> Tareas { get; set; }
}