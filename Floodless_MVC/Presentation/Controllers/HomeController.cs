using System.Diagnostics;
using Floodless_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Floodless_MVC.Application.Interfaces;
using Floodless_MVC.Domain.Enums;

namespace Floodless_MVC.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVoluntarioApplication _voluntarioApplication;
        private readonly IRecursoApplication _recursoApplication;

        public HomeController(
            ILogger<HomeController> logger,
            IVoluntarioApplication voluntarioApplication,
            IRecursoApplication recursoApplication)
        {
            _logger = logger;
            _voluntarioApplication = voluntarioApplication;
            _recursoApplication = recursoApplication;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var dashboard = new DashboardViewModel();

            try
            {
                // Dados reais do sistema
                var voluntarios = _voluntarioApplication.ObterTodos().ToList();
                var recursos = _recursoApplication.ObterTodos().ToList();

                dashboard.TotalVoluntarios = voluntarios.Count;
                dashboard.TotalRecursos = recursos.Count;
                dashboard.DoacoesMes = recursos.Count(r => r.DataCriacao.Month == DateTime.Now.Month);
                dashboard.AreasAtendidas = voluntarios
                    .Select(v => v.Contato)
                    .Where(c => !string.IsNullOrEmpty(c))
                    .Distinct()
                    .Count();

                // Dados para o gráfico de recursos por categoria
                var dadosRecursos = new int[5];
                foreach (var recurso in recursos)
                {
                    switch (recurso.TipoRecurso)
                    {
                        case TipoRecurso.Alimento:
                            dadosRecursos[0]++;
                            break;
                        case TipoRecurso.Remedio:
                            dadosRecursos[1]++;
                            break;
                        case TipoRecurso.Roupa:
                            dadosRecursos[2]++;
                            break;
                        default:
                            dadosRecursos[4]++;
                            break;
                    }
                }
                dashboard.DadosRecursos = dadosRecursos;

                // Dados para o gráfico de voluntários por região
                var dadosVoluntarios = new int[5];
                foreach (var voluntario in voluntarios)
                {
                    if (string.IsNullOrEmpty(voluntario.Contato))
                    {
                        dadosVoluntarios[4]++; // Centro/Não especificado
                    }
                    else if (voluntario.Contato.Contains("Norte", StringComparison.OrdinalIgnoreCase))
                        dadosVoluntarios[0]++;
                    else if (voluntario.Contato.Contains("Sul", StringComparison.OrdinalIgnoreCase))
                        dadosVoluntarios[1]++;
                    else if (voluntario.Contato.Contains("Leste", StringComparison.OrdinalIgnoreCase))
                        dadosVoluntarios[2]++;
                    else if (voluntario.Contato.Contains("Oeste", StringComparison.OrdinalIgnoreCase))
                        dadosVoluntarios[3]++;
                    else
                        dadosVoluntarios[4]++; // Centro/Não especificado
                }
                dashboard.DadosVoluntarios = dadosVoluntarios;

                // Atividades recentes baseadas em recursos e voluntários reais
                var atividadesRecentes = new List<AtividadeRecente>();
                var recursosRecentes = recursos.OrderByDescending(r => r.DataCriacao).Take(4);
                foreach (var recurso in recursosRecentes)
                {
                    atividadesRecentes.Add(new AtividadeRecente
                    {
                        Data = recurso.DataCriacao,
                        Descricao = $"Novo recurso: {recurso.Nome}",
                        Responsavel = recurso.Voluntario?.Nome ?? "Sistema",
                        Status = "Concluído",
                        StatusClass = "success"
                    });
                }
                dashboard.AtividadesRecentes = atividadesRecentes;

                // Recursos necessários baseados em dados reais
                var recursosNecessarios = recursos
                    .GroupBy(r => r.TipoRecurso)
                    .Select(g => new RecursoNecessario
                    {
                        Nome = g.Key.ToString(),
                        Quantidade = g.Sum(r => r.Quantidade)
                    })
                    .OrderByDescending(r => r.Quantidade)
                    .Take(4)
                    .ToList();
                dashboard.RecursosNecessarios = recursosNecessarios;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar dados do dashboard");
                // Não retornamos null aqui, continuamos com o dashboard vazio
            }

            return View(dashboard);
        }

        public IActionResult SobreEnchentes()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
