namespace GestionTareasUniversitarias.Models;

public class AdminViewModel
{
    public int TotalUsuarios { get; set; }
    public int TotalMaterias { get; set; }
    public int TotalTareas { get; set; }
    public int TareasCompletadas { get; set; }
    public int TasaCompletadas { get; set; }

    public List<UsuarioAdmin> Usuarios { get; set; } = new();
}

public class UsuarioAdmin
{
    public string Id { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public int Materias { get; set; }
    public int Tareas { get; set; }
    public bool Bloqueado { get; set; }
}
