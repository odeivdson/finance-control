using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace Web.Pages.Categorias
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IMapper _mapper;
        public DeleteModel(ICategoriaRepository categoriaRepository, ILogger<DeleteModel> logger, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _logger = logger;
            _mapper = mapper;
        }
        [BindProperty]
        public CategoriaViewModel Categoria { get; set; } = new();
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
                _logger.LogError(ex, "Erro ao carregar categoria para exclusão");
                TempData["Error"] = "Erro ao carregar categoria.";
                return RedirectToPage("Index");
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _categoriaRepository.DeleteAsync(Categoria.Id);
                TempData["Success"] = "Categoria excluída com sucesso!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir categoria");
                TempData["Error"] = "Erro ao excluir categoria.";
                return Page();
            }
        }
    }
}
