using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace Web.Pages.Metas
{
    public class CreateModel : PageModel
    {
        private readonly IMetaRepository _metaRepository;
        private readonly ILogger<CreateModel> _logger;
        private readonly IMapper _mapper;
        public CreateModel(IMetaRepository metaRepository, ILogger<CreateModel> logger, IMapper mapper)
        {
            _metaRepository = metaRepository;
            _logger = logger;
            _mapper = mapper;
        }
        [BindProperty]
        public MetaViewModel Meta { get; set; } = new();
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
                Meta.Id = Guid.NewGuid();
                var dto = _mapper.Map<MetaDto>(Meta);
                await _metaRepository.AddAsync(dto);
                TempData["Success"] = "Meta criada com sucesso!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar meta");
                TempData["Error"] = "Erro ao criar meta.";
                return Page();
            }
        }
    }
}
