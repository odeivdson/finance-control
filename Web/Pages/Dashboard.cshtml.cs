using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace Web.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ITransacaoService _transacaoService;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMetaRepository _metaRepository;
        private readonly IParcelaRepository _parcelaRepository;
        private readonly ILogger<DashboardModel> _logger;

        public DashboardModel(ITransacaoService transacaoService, ICategoriaRepository categoriaRepository, IMetaRepository metaRepository, IParcelaRepository parcelaRepository, ILogger<DashboardModel> logger)
        {
            _transacaoService = transacaoService;
            _categoriaRepository = categoriaRepository;
            _metaRepository = metaRepository;
            _parcelaRepository = parcelaRepository;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? DataInicio { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? DataFim { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Tipo { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Status { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid? CategoriaId { get; set; }
        public List<SelectListItem> CategoriasFiltro { get; set; } = new();
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public List<DashboardResumoMensal> ResumoMensal { get; set; } = new();
        public DashboardResumoMensal? MesAtualResumo { get; set; }
        public DashboardResumoMensal? MesAnteriorResumo { get; set; }

        public class DashboardResumoMensal
        {
            public int Ano { get; set; }
            public int Mes { get; set; }
            public decimal Receitas { get; set; }
            public decimal Despesas { get; set; }
        }

        public async Task OnGetAsync()
        {
            try
            {
                var categoriasList = (await _categoriaRepository.GetAllAsync()).ToList();
                CategoriasFiltro = categoriasList.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nome }).ToList();

                var filtro = new TransacaoFiltroDto
                {
                    DataInicio = DataInicio,
                    DataFim = DataFim?.Date.AddDays(1).AddTicks(-1),
                    Tipo = Tipo,
                    Status = Status,
                    CategoriaId = CategoriaId
                };

                var transacoes = (await _transacaoService.GetAllAsync(filtro)).ToList();

                TotalReceitas = transacoes.Where(t => t.Tipo == 1).Sum(t => t.Valor);
                TotalDespesas = transacoes.Where(t => t.Tipo == 2).Sum(t => t.Valor);

                var anoAtual = DateTime.Now.Year;
                var mesAtual = DateTime.Now.Month;
                var anoAnterior = mesAtual == 1 ? anoAtual - 1 : anoAtual;
                var mesAnterior = mesAtual == 1 ? 12 : mesAtual - 1;

                // Resumo mensal para gráficos
                ResumoMensal = transacoes
                    .GroupBy(t => new { t.Data.Year, t.Data.Month })
                    .Select(g => new DashboardResumoMensal
                    {
                        Ano = g.Key.Year,
                        Mes = g.Key.Month,
                        Receitas = g.Where(t => t.Tipo == 1).Sum(t => t.Valor),
                        Despesas = g.Where(t => t.Tipo == 2).Sum(t => t.Valor)
                    })
                    .OrderBy(x => x.Ano).ThenBy(x => x.Mes)
                    .ToList();

                MesAtualResumo = ResumoMensal.FirstOrDefault(x => x.Ano == anoAtual && x.Mes == mesAtual);
                MesAnteriorResumo = ResumoMensal.FirstOrDefault(x => x.Ano == anoAnterior && x.Mes == mesAnterior);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar o dashboard.");
            }
        }
    }
}