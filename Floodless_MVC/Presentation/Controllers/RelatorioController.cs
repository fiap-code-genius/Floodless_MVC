using Floodless_MVC.Application.Interfaces;
using Floodless_MVC.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Floodless_MVC.Presentation.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly IRecursoApplication _recursoApplication;
        private readonly RelatorioExcelService _relatorioService;

        public RelatorioController(IRecursoApplication recursoApplication, RelatorioExcelService relatorioService)
        {
            _recursoApplication = recursoApplication;
            _relatorioService = relatorioService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GerarExcel()
        {
            try
            {
                var recursos = _recursoApplication.ObterTodos().ToList();
                
                if (!recursos.Any())
                {
                    TempData["Warning"] = "Não há recursos cadastrados para gerar o relatório.";
                    return RedirectToAction("Index", "Recurso");
                }

                var excelBytes = await _relatorioService.GerarRelatorioExcelAsync(recursos);
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"Relatorio_Recursos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                TempData["Success"] = "Relatório gerado com sucesso!";
                return File(excelBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao gerar relatório: {ex.Message}";
                return RedirectToAction("Index", "Recurso");
            }
        }
    }
}
