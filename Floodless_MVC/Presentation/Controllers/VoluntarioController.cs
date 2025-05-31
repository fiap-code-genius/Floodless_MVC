using Floodless_MVC.Application.Interfaces;
using Floodless_MVC.Application.Voluntario;
using Microsoft.AspNetCore.Mvc;

namespace Floodless_MVC.Presentation.Controllers
{
    public class VoluntarioController : Controller
    {
        private readonly IVoluntarioApplication _application;

        public VoluntarioController(IVoluntarioApplication application)
        {
            _application = application;
        }

        public IActionResult Index()
        {
            var voluntarios = _application.ObterTodos();
            return View(voluntarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VoluntarioDto voluntario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _application.SalvarDados(voluntario);
                    if (result != null)
                    {
                        TempData["Success"] = $"Voluntário cadastrado com sucesso! ID do voluntário: {result.Id}";
                        TempData["VoluntarioId"] = result.Id;
                        TempData["VoluntarioNome"] = result.Nome;
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View(voluntario);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(voluntario);
            }
        }

        public IActionResult Edit(int id)
        {
            var voluntario = _application.ObterPorId(id);
            if (voluntario == null)
            {
                return NotFound();
            }

            var voluntarioDto = new VoluntarioEditDto
            {
                Id = voluntario.Id,
                Nome = voluntario.Nome,
                Email = voluntario.Email,
                Contato = voluntario.Contato
            };

            return View(voluntarioDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, VoluntarioEditDto voluntario)
        {
            try
            {
                if (id != voluntario.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var result = _application.EditarDados(id, voluntario);
                    if (result != null)
                    {
                        TempData["Success"] = "Voluntário atualizado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View(voluntario);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(voluntario);
            }
        }

        public IActionResult Details(int id)
        {
            var voluntario = _application.ObterPorId(id);
            if (voluntario == null)
            {
                return NotFound();
            }
            return PartialView("_DetailsPartial", voluntario);
        }

        public IActionResult Delete(int id)
        {
            var voluntario = _application.ObterPorId(id);
            if (voluntario == null)
            {
                return NotFound();
            }
            return View(voluntario);
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
                    TempData["Success"] = "Voluntário excluído com sucesso!";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
