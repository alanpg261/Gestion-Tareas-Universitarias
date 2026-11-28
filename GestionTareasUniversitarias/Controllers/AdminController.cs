using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTareasUniversitarias.Data;
using GestionTareasUniversitarias.Models;

namespace GestionTareasUniversitarias.Controllers;

[Authorize(Roles = "Administrador")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _userManager.Users.ToListAsync();

        var materiasPorUsuario = (await _context.Materias
            .GroupBy(m => m.UsuarioId)
            .Select(g => new { UsuarioId = g.Key, Total = g.Count() })
            .ToListAsync())
            .ToDictionary(x => x.UsuarioId, x => x.Total);

        var tareasPorUsuario = (await _context.Tareas
            .GroupBy(t => t.Materia.UsuarioId)
            .Select(g => new { UsuarioId = g.Key, Total = g.Count() })
            .ToListAsync())
            .ToDictionary(x => x.UsuarioId, x => x.Total);

        var vm = new AdminViewModel
        {
            TotalUsuarios = usuarios.Count,
            TotalMaterias = materiasPorUsuario.Values.Sum(),
            TotalTareas = await _context.Tareas.CountAsync(),
            TareasCompletadas = await _context.Tareas.CountAsync(t => t.Estado == EstadoTarea.Completada)
        };

        vm.TasaCompletadas = vm.TotalTareas == 0
            ? 0
            : (int)Math.Round(100.0 * vm.TareasCompletadas / vm.TotalTareas);

        foreach (var usuario in usuarios.OrderBy(u => u.Email))
        {
            var roles = await _userManager.GetRolesAsync(usuario);
            vm.Usuarios.Add(new UsuarioAdmin
            {
                Id = usuario.Id,
                Correo = usuario.Email ?? "(sin correo)",
                Rol = roles.FirstOrDefault() ?? "(sin rol)",
                Materias = materiasPorUsuario.GetValueOrDefault(usuario.Id, 0),
                Tareas = tareasPorUsuario.GetValueOrDefault(usuario.Id, 0),
                Bloqueado = usuario.LockoutEnabled
                    && usuario.LockoutEnd.HasValue
                    && usuario.LockoutEnd.Value > DateTimeOffset.UtcNow
            });
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Bloquear(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario != null && !EsPropio(usuario.Id))
        {
            await _userManager.SetLockoutEnabledAsync(usuario, true);
            await _userManager.SetLockoutEndDateAsync(usuario, DateTimeOffset.UtcNow.AddYears(100));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Desbloquear(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario != null)
        {
            await _userManager.SetLockoutEndDateAsync(usuario, null);
        }
        return RedirectToAction(nameof(Index));
    }

    private bool EsPropio(string id)
    {
        return _userManager.GetUserId(User) == id;
    }
}
