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

    public EstadoTarea Estado { get; set; } = EstadoTarea.Pendiente;

    public int MateriaId { get; set; }

    public Materia? Materia { get; set; }
}
