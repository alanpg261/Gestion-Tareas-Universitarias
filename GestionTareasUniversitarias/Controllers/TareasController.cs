using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionTareasUniversitarias.Data;
using GestionTareasUniversitarias.Models;

namespace GestionTareasUniversitarias.Controllers
{
    [Authorize]
    public class TareasController : Controller
    {
        private readonly AppDbContext _context;

        public TareasController(AppDbContext context)
        {
            _context = context;
        }

        private string UsuarioId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // GET: Tareas
        public async Task<IActionResult> Index(int? materiaId, int? estado, string? buscar)
        {
            var query = _context.Tareas
                .Include(t => t.Materia)
                .Where(t => t.Materia.UsuarioId == UsuarioId);

            if (materiaId.HasValue)
            {
                query = query.Where(t => t.MateriaId == materiaId);
            }
            if (estado == 99)
            {
                query = query.Where(t => t.Estado != EstadoTarea.Completada && t.FechaLimite < DateTime.Now);
            }
            else if (estado.HasValue && Enum.IsDefined(typeof(EstadoTarea), estado.Value))
            {
                query = query.Where(t => (int)t.Estado == estado.Value);
            }
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                query = query.Where(t => t.Titulo.Contains(buscar)
                    || (t.Descripcion != null && t.Descripcion.Contains(buscar)));
            }

            var tareas = await query
                .OrderBy(t => t.FechaLimite)
                .ToListAsync();

            ViewData["MateriaId"] = MateriasSelectList(materiaId);
            ViewData["MateriaIdSeleccionada"] = materiaId;
            ViewData["EstadoFiltro"] = estado;
            ViewData["Buscar"] = buscar;

            return View(tareas);
        }

        // GET: Tareas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await BuscarTareaPropiaAsync(id.Value);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // GET: Tareas/Create
        public IActionResult Create()
        {
            ViewData["MateriaId"] = MateriasSelectList();
            return View();
        }

        // POST: Tareas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descripcion,FechaLimite,Estado,MateriaId")] Tarea tarea)
        {
            await ValidarMateriaPropiaAsync(tarea);

            if (ModelState.IsValid)
            {
                _context.Add(tarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MateriaId"] = MateriasSelectList(tarea.MateriaId);
            return View(tarea);
        }

        // GET: Tareas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await BuscarTareaPropiaAsync(id.Value);
            if (tarea == null)
            {
                return NotFound();
            }
            ViewData["MateriaId"] = MateriasSelectList(tarea.MateriaId);
            return View(tarea);
        }

        // POST: Tareas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,FechaLimite,Estado,MateriaId")] Tarea tarea)
        {
            if (id != tarea.Id)
            {
                return NotFound();
            }

            var tareaExistente = await BuscarTareaPropiaAsync(id);
            if (tareaExistente == null)
            {
                return NotFound();
            }

            await ValidarMateriaPropiaAsync(tarea);

            if (await TryUpdateModelAsync(tareaExistente, "", t => t.Titulo, t => t.Descripcion,
                t => t.FechaLimite, t => t.Estado, t => t.MateriaId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TareaExists(tarea.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MateriaId"] = MateriasSelectList(tarea.MateriaId);
            return View(tarea);
        }

        // GET: Tareas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await BuscarTareaPropiaAsync(id.Value);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // POST: Tareas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await BuscarTareaPropiaAsync(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private Task<Tarea?> BuscarTareaPropiaAsync(int id)
        {
            return _context.Tareas
                .Include(t => t.Materia)
                .FirstOrDefaultAsync(t => t.Id == id && t.Materia.UsuarioId == UsuarioId);
        }

        private SelectList MateriasSelectList(object? seleccionada = null)
        {
            var materias = _context.Materias
                .Where(m => m.UsuarioId == UsuarioId)
                .OrderBy(m => m.Nombre);
            return new SelectList(materias, "Id", "Nombre", seleccionada);
        }

        private async Task ValidarMateriaPropiaAsync(Tarea tarea)
        {
            var materiaEsPropia = await _context.Materias
                .AnyAsync(m => m.Id == tarea.MateriaId && m.UsuarioId == UsuarioId);
            if (!materiaEsPropia)
            {
                ModelState.AddModelError("MateriaId", "Debe seleccionar una materia propia.");
            }
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.Id == id && e.Materia.UsuarioId == UsuarioId);
        }
    }
}
