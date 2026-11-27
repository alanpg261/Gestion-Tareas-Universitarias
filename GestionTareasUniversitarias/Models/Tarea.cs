using System.ComponentModel.DataAnnotations;

namespace GestionTareasUniversitarias.Models;

public class Tarea
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; set; }

    [Required]
    public DateTime FechaLimite { get; set; }

    public bool Completada { get; set; }

    public int MateriaId { get; set; }

    public Materia? Materia { get; set; }
}
