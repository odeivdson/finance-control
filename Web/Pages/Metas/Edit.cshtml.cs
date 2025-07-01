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
    public class EditModel : PageModel
    {
        private readonly IMetaRepository _metaRepository;
        private readonly ILogger<EditModel> _logger;
        private readonly IMapper _mapper;
        public EditModel(IMetaRepository metaRepository, ILogger<EditModel> logger, IMapper mapper)
        {
            _metaRepository = metaRepository;
            _logger = logger;
            _mapper = mapper;
        }
        [BindProperty]
        public MetaViewModel Meta { get; set; } = new();
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var meta = await _metaRepository.GetByIdAsync(id);
                if (meta == null)
                {
                    TempData["Error"] = "Meta não encontrada.";
                    return RedirectToPage("Index");
                }
                Meta = _mapper.Map<MetaViewModel>(meta);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar meta para edição");
                TempData["Error"] = "Erro ao carregar meta.";
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
                var dto = _mapper.Map<MetaDto>(Meta);
                await _metaRepository.UpdateAsync(dto);
                TempData["Success"] = "Meta editada com sucesso!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar meta");
                TempData["Error"] = "Erro ao editar meta.";
                return Page();
            }
        }
    }
}
