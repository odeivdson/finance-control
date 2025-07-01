using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Web.Pages.Categorias
{
    public class IndexModel : PageModel
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ICategoriaRepository categoriaRepository, ILogger<IndexModel> logger)
        {
            _categoriaRepository = categoriaRepository;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<CategoriaDto> Categorias { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                var allCategorias = (await _categoriaRepository.GetAllAsync()).ToList();
                TotalCount = allCategorias.Count;
                TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
                Categorias = allCategorias
                    .OrderBy(c => c.Nome)
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar categorias");
                TempData["Error"] = "Erro ao carregar categorias.";
                Categorias = new List<CategoriaDto>();
            }
        }

        public string GetQueryString()
        {
            // Adapte se houver filtros futuramente
            return string.Empty;
        }
    }
}
