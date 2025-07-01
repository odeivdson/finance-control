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
    public class EditModel : PageModel
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<EditModel> _logger;
        private readonly IMapper _mapper;
        public EditModel(ICategoriaRepository categoriaRepository, ILogger<EditModel> logger, IMapper mapper)
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
                _logger.LogError(ex, "Erro ao carregar categoria para edição");
                TempData["Error"] = "Erro ao carregar categoria.";
                return RedirectToPage("Index");
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                return Page();
            }
            try
            {
                var dto = _mapper.Map<CategoriaDto>(Categoria);
                await _categoriaRepository.UpdateAsync(dto);
                TempData["Success"] = "Categoria editada com sucesso!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar categoria");
                TempData["Error"] = "Erro ao editar categoria.";
                return Page();
            }
        }
    }
}
