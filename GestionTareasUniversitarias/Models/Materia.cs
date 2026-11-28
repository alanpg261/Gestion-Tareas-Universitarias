using System.ComponentModel.DataAnnotations;

namespace GestionTareasUniversitarias.Models;

public class Materia
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Descripcion { get; set; }

    [Required]
    public string UsuarioId { get; set; } = string.Empty;

    public ApplicationUser? Usuario { get; set; }

    public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}