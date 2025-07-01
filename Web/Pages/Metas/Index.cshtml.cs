using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Web.Pages.Metas
{
    public class IndexModel : PageModel
    {
        private readonly IMetaRepository _metaRepository;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(IMetaRepository metaRepository, ILogger<IndexModel> logger)
        {
            _metaRepository = metaRepository;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<MetaDto> Metas { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                var allMetas = (await _metaRepository.GetAllAsync()).ToList();
                TotalCount = allMetas.Count;
                TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
                Metas = allMetas
                    .OrderByDescending(m => m.DataLimite)
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar metas");
                TempData["Error"] = "Erro ao carregar metas.";
                Metas = new List<MetaDto>();
            }
        }

        public string GetQueryString()
        {
            // Adapte se houver filtros futuramente
            return string.Empty;
        }
    }
}
