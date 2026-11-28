namespace GestionTareasUniversitarias.Models;

public class TareaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime FechaLimite { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Materia { get; set; } = string.Empty;
    public bool Vencida { get; set; }
}

public class EstadisticasDto
{
    public int Total { get; set; }
    public int Pendientes { get; set; }
    public int EnProgreso { get; set; }
    public int Completadas { get; set; }
    public int Vencidas { get; set; }
    public int TasaCompletadas { get; set; }
}
