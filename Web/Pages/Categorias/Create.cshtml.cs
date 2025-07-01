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
    public class CreateModel : PageModel
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<CreateModel> _logger;
        private readonly IMapper _mapper;
        public CreateModel(ICategoriaRepository categoriaRepository, ILogger<CreateModel> logger, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _logger = logger;
            _mapper = mapper;
        }
        [BindProperty]
        public CategoriaViewModel Categoria { get; set; } = new();
        public void OnGet() { }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                return Page();
            }
            try
            {
                Categoria.Id = Guid.NewGuid();
                var dto = _mapper.Map<CategoriaDto>(Categoria);
                await _categoriaRepository.AddAsync(dto);
                TempData["Success"] = "Categoria criada com sucesso!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar categoria");
                TempData["Error"] = "Erro ao criar categoria.";
                return Page();
            }
        }
    }
}
