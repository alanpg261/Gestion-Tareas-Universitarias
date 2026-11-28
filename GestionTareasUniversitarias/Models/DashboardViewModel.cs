namespace GestionTareasUniversitarias.Models;

public class DashboardViewModel
{
    public int Pendientes { get; set; }
    public int EnProgreso { get; set; }
    public int Completadas { get; set; }
    public int Vencidas { get; set; }
    public int ProximasAVencer { get; set; }
    public int TasaCompletadas { get; set; }

    public List<Tarea> ListaRecientes { get; set; } = new();
}
