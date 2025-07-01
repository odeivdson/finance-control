using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace Web.Pages.Categorias
{
    public class DetailsModel : PageModel
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<DetailsModel> _logger;
        private readonly IMapper _mapper;
        public DetailsModel(ICategoriaRepository categoriaRepository, ILogger<DetailsModel> logger, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public CategoriaViewModel? Categoria { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var categoria = await _categoriaRepository.GetByIdAsync(id);
                if (categoria == null)
                {
                    TempData["Error"] = "Categoria não encontrada.";
                    return RedirectToPage("Index");
                }
                Categoria = _mapper.Map<CategoriaViewModel>(categoria);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar detalhes da categoria");
                TempData["Error"] = "Erro ao carregar detalhes da categoria.";
                return RedirectToPage("Index");
            }
        }
    }
}
