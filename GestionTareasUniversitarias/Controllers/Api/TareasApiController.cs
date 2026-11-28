using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTareasUniversitarias.Data;
using GestionTareasUniversitarias.Models;

namespace GestionTareasUniversitarias.Controllers.Api;

[Authorize(Roles = "Estudiante")]
[ApiController]
[Route("api/tareas")]
public class TareasApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public TareasApiController(AppDbContext context)
    {
        _context = context;
    }

    private string UsuarioId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private IQueryable<Tarea> TareasPropias =>
        _context.Tareas.Where(t => t.Materia.UsuarioId == UsuarioId);

    private static TareaDto MapearDto(Tarea tarea, DateTime ahora) => new()
    {
        Id = tarea.Id,
        Titulo = tarea.Titulo,
        Descripcion = tarea.Descripcion,
        FechaLimite = tarea.FechaLimite,
        Estado = tarea.Estado.ToString(),
        Materia = tarea.Materia?.Nombre ?? string.Empty,
        Vencida = tarea.Estado != EstadoTarea.Completada && tarea.FechaLimite < ahora
    };

    /// <summary>Lista las tareas del estudiante autenticado.</summary>
    [HttpGet]
    public async Task<IActionResult> GetTareas()
    {
        var ahora = DateTime.Now;
        var tareas = await TareasPropias
            .Include(t => t.Materia)
            .OrderBy(t => t.FechaLimite)
            .ToListAsync();
        return Ok(tareas.Select(t => MapearDto(t, ahora)));
    }

    /// <summary>Lista las tareas del estudiante que vencen en las proximas 72 horas.</summary>
    [HttpGet("proximas-a-vencer")]
    public async Task<IActionResult> GetProximasAVencer()
    {
        var ahora = DateTime.Now;
        var tareas = await TareasPropias
            .Include(t => t.Materia)
            .Where(t => t.Estado != EstadoTarea.Completada
                && t.FechaLimite >= ahora
                && t.FechaLimite <= ahora.AddHours(72))
            .OrderBy(t => t.FechaLimite)
            .ToListAsync();
        return Ok(tareas.Select(t => MapearDto(t, ahora)));
    }

    /// <summary>Devuelve las estadisticas de tareas del estudiante autenticado.</summary>
    [HttpGet("estadisticas")]
    public async Task<IActionResult> GetEstadisticas()
    {
        var ahora = DateTime.Now;
        var activas = TareasPropias.Where(t => t.Estado != EstadoTarea.Completada);

        var dto = new EstadisticasDto
        {
            Total = await TareasPropias.CountAsync(),
            Pendientes = await activas.CountAsync(t => t.Estado == EstadoTarea.Pendiente && t.FechaLimite >= ahora),
            EnProgreso = await activas.CountAsync(t => t.Estado == EstadoTarea.EnProgreso),
            Completadas = await TareasPropias.CountAsync(t => t.Estado == EstadoTarea.Completada),
            Vencidas = await activas.CountAsync(t => t.FechaLimite < ahora)
        };
        dto.TasaCompletadas = dto.Total == 0
            ? 0
            : (int)Math.Round(100.0 * dto.Completadas / dto.Total);

        return Ok(dto);
    }

    /// <summary>Marca como completada una tarea propia del estudiante autenticado.</summary>
    [HttpPost("{id:int}/completar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostCompletar(int id)
    {
        var tarea = await TareasPropias
            .Include(t => t.Materia)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (tarea == null)
        {
            return NotFound(new { mensaje = "La tarea no existe o no pertenece al estudiante." });
        }

        tarea.Estado = EstadoTarea.Completada;
        await _context.SaveChangesAsync();

        return Ok(MapearDto(tarea, DateTime.Now));
    }
}
