using Floodless_MVC.Application.Services;
using Floodless_MVC.Infrastructure.Data.AppData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Floodless_MVC.Presentation.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly RelatorioExcelService _service;

        public RelatorioController(ApplicationContext context, RelatorioExcelService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> BaixarRelatorioRecursos()
        {
            var recursos = await _context.Recurso.Include(r => r.Voluntario).ToListAsync();

            var excelBytes = await _service.GerarRelatorioExcelAsync(recursos);

            return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Relatorio_Recursos.xlsx"
                );
        }
    }
}
