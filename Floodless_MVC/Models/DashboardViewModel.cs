using System;
using System.Collections.Generic;
using System.Linq;
using Floodless_MVC.Domain.Entities;
using Floodless_MVC.Domain.Enums;

namespace Floodless_MVC.Models
{
    public class DashboardViewModel
    {
        public int TotalVoluntarios { get; set; }
        public int TotalRecursos { get; set; }
        public int DoacoesMes { get; set; }
        public int AreasAtendidas { get; set; }

        public int[] DadosRecursos { get; set; } = new int[5];
        public string[] CategoriasRecursos { get; set; }
        public int[] DadosVoluntarios { get; set; } = new int[5];
        public string[] RegioesCidade { get; set; }

        public List<AtividadeRecente> AtividadesRecentes { get; set; }
        public List<RecursoNecessario> RecursosNecessarios { get; set; }

        public DashboardViewModel()
        {
            AtividadesRecentes = new List<AtividadeRecente>();
            RecursosNecessarios = new List<RecursoNecessario>();
            CategoriasRecursos = new[] { "Alimento", "Remédio", "Roupa", "Não Classificado", "Outros" };
            RegioesCidade = new[] { "Norte", "Sul", "Leste", "Oeste", "Centro" };
        }
    }

    public class AtividadeRecente
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusClass { get; set; } = string.Empty;
    }

    public class RecursoNecessario
    {
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
} 