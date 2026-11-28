using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTareasUniversitarias.Data;
using GestionTareasUniversitarias.Models;

namespace GestionTareasUniversitarias.Controllers;

[Authorize]
public class EstadisticasController : Controller
{
    private readonly AppDbContext _context;

    public EstadisticasController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var ahora = DateTime.Now;

        var materias = await _context.Materias
            .Include(m => m.Tareas)
            .Where(m => m.UsuarioId == usuarioId)
            .ToListAsync();

        var vm = new EstadisticasViewModel();

        foreach (var materia in materias)
        {
            var stats = new MateriaEstadistica
            {
                Nombre = materia.Nombre,
                Total = materia.Tareas.Count,
                Completadas = materia.Tareas.Count(t => t.Estado == EstadoTarea.Completada),
                EnProgreso = materia.Tareas.Count(t => t.Estado == EstadoTarea.EnProgreso),
                Pendientes = materia.Tareas.Count(t =>
                    t.Estado == EstadoTarea.Pendiente && t.FechaLimite >= ahora),
                Vencidas = materia.Tareas.Count(t =>
                    t.Estado != EstadoTarea.Completada && t.FechaLimite < ahora)
            };
            vm.PorMateria.Add(stats);

            vm.TotalTareas += stats.Total;
            vm.Completadas += stats.Completadas;
            vm.EnProgreso += stats.EnProgreso;
            vm.Pendientes += stats.Pendientes;
            vm.Vencidas += stats.Vencidas;
        }

        vm.TasaCompletadas = vm.TotalTareas == 0
            ? 0
            : (int)Math.Round(100.0 * vm.Completadas / vm.TotalTareas);

        return View(vm);
    }
}
