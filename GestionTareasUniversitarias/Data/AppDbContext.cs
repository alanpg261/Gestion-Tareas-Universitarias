using GestionTareasUniversitarias.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GestionTareasUniversitarias.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>

{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Materia> Materias { get; set; }

    public DbSet<Tarea> Tareas { get; set; }
}