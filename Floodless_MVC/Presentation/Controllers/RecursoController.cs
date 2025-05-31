using Floodless_MVC.Application.Dtos.Recurso;
using Floodless_MVC.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Floodless_MVC.Presentation.Controllers
{
    public class RecursoController : Controller
    {
        private readonly IRecursoApplication _application;

        public RecursoController(IRecursoApplication application)
        {
            _application = application;
        }

        public IActionResult Index(int page = 1)
        {
            var recursos = _application.ObterTodos().ToList();
            const int ItemsPerPage = 10;
            
            var totalItems = recursos.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)ItemsPerPage);
            
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));
            
            var recursosPaginados = recursos
                .Skip((page - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList();
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            
            return View(recursosPaginados);
        }

        public IActionResult Create(int? voluntarioId = null)
        {
            if (voluntarioId.HasValue)
            {
                ViewBag.VoluntarioId = voluntarioId.Value;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RecursoDto recurso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _application.SalvarDados(recurso);
                    if (result != null)
                    {
                        TempData["Success"] = "Recurso cadastrado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View(recurso);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(recurso);
            }
        }

        public IActionResult Edit(int id)
        {
            var recurso = _application.ObterPorId(id);
            if (recurso == null)
            {
                return NotFound();
            }

            var recursoDto = new RecursoDto
            {
                Nome = recurso.Nome,
                TipoRecurso = recurso.TipoRecurso,
                Quantidade = recurso.Quantidade,
                VoluntarioId = recurso.VoluntarioId
            };

            return View(recursoDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, RecursoDto recurso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _application.EditarDados(id, recurso);
                    if (result != null)
                    {
                        TempData["Success"] = "Recurso atualizado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View(recurso);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(recurso);
            }
        }

        public IActionResult Delete(int id)
        {
            var recurso = _application.ObterPorId(id);
            if (recurso == null)
            {
                return NotFound();
            }
            return View(recurso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var result = _application.DeletarDados(id);
                if (result != null)
                {
                    TempData["Success"] = "Recurso excluído com sucesso!";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id)
        {
            var recurso = _application.ObterPorId(id);
            if (recurso == null)
            {
                return NotFound();
            }
            return View(recurso);
        }
    }
}
