using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using AutoMapper;
using System;

namespace Web.Pages.Metas
{
    public class DetailsModel : PageModel
    {
        private readonly IMetaRepository _metaRepository;
        private readonly ILogger<DetailsModel> _logger;
        private readonly IMapper _mapper;
        public DetailsModel(IMetaRepository metaRepository, ILogger<DetailsModel> logger, IMapper mapper)
        {
            _metaRepository = metaRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public MetaViewModel? Meta { get; set; }
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
                _logger.LogError(ex, "Erro ao carregar detalhes da meta");
                TempData["Error"] = "Erro ao carregar detalhes da meta.";
                return RedirectToPage("Index");
            }
        }
    }
}
