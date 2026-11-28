using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTareasUniversitarias.Data;
using GestionTareasUniversitarias.Models;

namespace GestionTareasUniversitarias.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Administrador"))
        {
            return RedirectToAction("Index", "Admin");
        }

        var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var ahora = DateTime.Now;

        var tareas = _context.Tareas.Where(t => t.Materia.UsuarioId == usuarioId);
        var activasNoCompletadas = tareas.Where(t => t.Estado != EstadoTarea.Completada);

        var vm = new DashboardViewModel
        {
            Pendientes = await activasNoCompletadas.CountAsync(t => t.Estado == EstadoTarea.Pendiente && t.FechaLimite >= ahora),
            EnProgreso = await activasNoCompletadas.CountAsync(t => t.Estado == EstadoTarea.EnProgreso),
            Completadas = await tareas.CountAsync(t => t.Estado == EstadoTarea.Completada),
            Vencidas = await activasNoCompletadas.CountAsync(t => t.FechaLimite < ahora),
            ProximasAVencer = await activasNoCompletadas.CountAsync(t =>
                t.FechaLimite >= ahora && t.FechaLimite <= ahora.AddHours(72)),
            ListaRecientes = await tareas
                .Include(t => t.Materia)
                .OrderByDescending(t => t.Id)
                .Take(5)
                .ToListAsync()
        };

        var total = await tareas.CountAsync();
        vm.TasaCompletadas = total == 0 ? 0 : (int)Math.Round(100.0 * vm.Completadas / total);

        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error404()
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
