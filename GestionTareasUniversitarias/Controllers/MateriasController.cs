using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTareasUniversitarias.Data;
using GestionTareasUniversitarias.Models;

namespace GestionTareasUniversitarias.Controllers
{
    [Authorize]
    public class MateriasController : Controller
    {
        private readonly AppDbContext _context;

        public MateriasController(AppDbContext context)
        {
            _context = context;
        }

        private string UsuarioId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // GET: Materias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Materias
                .Include(m => m.Tareas)
                .Where(m => m.UsuarioId == UsuarioId)
                .ToListAsync());
        }

        // GET: Materias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == UsuarioId);
            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        // GET: Materias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion")] Materia materia)
        {
            ModelState.Remove(nameof(materia.UsuarioId));
            if (ModelState.IsValid)
            {
                materia.UsuarioId = UsuarioId;
                _context.Add(materia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materia);
        }

        // GET: Materias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == UsuarioId);
            if (materia == null)
            {
                return NotFound();
            }
            return View(materia);
        }

        // POST: Materias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion")] Materia materia)
        {
            if (id != materia.Id)
            {
                return NotFound();
            }

            var materiaExistente = await _context.Materias
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == UsuarioId);
            if (materiaExistente == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(materiaExistente, "", m => m.Nombre, m => m.Descripcion))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaExists(materia.Id))
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
            return View(materia);
        }

        // GET: Materias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == UsuarioId);
            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        // POST: Materias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materia = await _context.Materias
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == UsuarioId);
            if (materia != null)
            {
                _context.Materias.Remove(materia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MateriaExists(int id)
        {
            return _context.Materias.Any(e => e.Id == id && e.UsuarioId == UsuarioId);
        }
    }
}
