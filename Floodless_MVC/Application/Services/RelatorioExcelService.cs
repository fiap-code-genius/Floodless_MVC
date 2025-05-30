using Floodless_MVC.Domain.Entities;
using OfficeOpenXml;

namespace Floodless_MVC.Application.Services
{
    public class RelatorioExcelService
    {
        public async Task<byte[]> GerarRelatorioExcelAsync(List<RecursoEntity> recursos)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var pacote = new ExcelPackage();
            var planilha = pacote.Workbook.Worksheets.Add("Recursos");

            //Cabeçalhos
            planilha.Cells[1, 1].Value = "ID";
            planilha.Cells[1, 2].Value = "Nome";
            planilha.Cells[1, 3].Value = "Tipo de Recurso";
            planilha.Cells[1, 4].Value = "Quantidade";
            planilha.Cells[1, 5].Value = "Voluntário";

            // Conteudo
            for (int i = 0; i < recursos.Count; i++)
            {
                var r = recursos[i];

                planilha.Cells[i + 2, 1].Value = r.Id;
                planilha.Cells[i + 2, 2].Value = r.Nome;
                planilha.Cells[i + 2, 3].Value = r.TipoRecurso;
                planilha.Cells[i + 2, 4].Value = r.Quantidade;
                planilha.Cells[i + 2, 5].Value = r.Voluntario?.Nome;
            }

            return await pacote.GetAsByteArrayAsync();
        }
    }
}
