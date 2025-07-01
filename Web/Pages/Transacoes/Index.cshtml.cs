using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Pages.Transacoes
{
    public class IndexModel : PageModel
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<IndexModel> _logger;
        private readonly IMapper _mapper;

        public IndexModel(ITransacaoRepository transacaoRepository, ICategoriaRepository categoriaRepository, ILogger<IndexModel> logger, IMapper mapper)
        {
            _transacaoRepository = transacaoRepository;
            _categoriaRepository = categoriaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public TransacaoFiltroViewModel Filtro { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<SelectListItem> CategoriasFiltro { get; set; } = new();
        public List<TransacaoViewModel> Transacoes { get; set; } = new();

        public async Task OnGetAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            CategoriasFiltro = categorias.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nome }).ToList();

            // Filtro padrão mês atual
            if (!Filtro.DataInicio.HasValue && !Filtro.DataFim.HasValue)
            {
                var now = DateTime.Now;
                Filtro.DataInicio = new DateTime(now.Year, now.Month, 1);
                Filtro.DataFim = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
            }

            var transacoes = await _transacaoRepository.GetAllAsync();
            var query = transacoes.AsQueryable();

            if (Filtro.DataInicio.HasValue)
                query = query.Where(t => t.Data >= Filtro.DataInicio.Value);
            if (Filtro.DataFim.HasValue)
                query = query.Where(t => t.Data <= Filtro.DataFim.Value);
            if (Filtro.Tipo.HasValue)
                query = query.Where(t => t.Tipo == Filtro.Tipo.Value);
            if (Filtro.Status.HasValue)
                query = query.Where(t => t.Status == Filtro.Status.Value);
            if (Filtro.CategoriaId.HasValue)
                query = query.Where(t => t.CategoriaId == Filtro.CategoriaId.Value);

            TotalCount = query.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Transacoes = _mapper.Map<List<TransacaoViewModel>>(query
                .OrderByDescending(t => t.Data)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList());
        }

        public string GetQueryString()
        {
            var query = new List<string>();
            if (Filtro.DataInicio.HasValue)
                query.Add($"DataInicio={Filtro.DataInicio:yyyy-MM-dd}");
            if (Filtro.DataFim.HasValue)
                query.Add($"DataFim={Filtro.DataFim:yyyy-MM-dd}");
            if (Filtro.Tipo.HasValue)
                query.Add($"Tipo={Filtro.Tipo}");
            if (Filtro.Status.HasValue)
                query.Add($"Status={Filtro.Status}");
            if (Filtro.CategoriaId.HasValue)
                query.Add($"CategoriaId={Filtro.CategoriaId}");
            return query.Count > 0 ? "&" + string.Join("&", query) : "";
        }
    }
}
