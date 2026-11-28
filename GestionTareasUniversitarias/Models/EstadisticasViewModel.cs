namespace GestionTareasUniversitarias.Models;

public class EstadisticasViewModel
{
    public int TotalTareas { get; set; }
    public int Pendientes { get; set; }
    public int EnProgreso { get; set; }
    public int Completadas { get; set; }
    public int Vencidas { get; set; }
    public int TasaCompletadas { get; set; }

    public List<MateriaEstadistica> PorMateria { get; set; } = new();
}

public class MateriaEstadistica
{
    public string Nombre { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Pendientes { get; set; }
    public int EnProgreso { get; set; }
    public int Completadas { get; set; }
    public int Vencidas { get; set; }
}
