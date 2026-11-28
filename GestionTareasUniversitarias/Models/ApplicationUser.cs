using Microsoft.AspNetCore.Identity;

namespace GestionTareasUniversitarias.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Materia> Materias { get; set; } = new List<Materia>();
}