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
    public class DeleteModel : PageModel
    {
        private readonly IMetaRepository _metaRepository;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IMapper _mapper;
        public DeleteModel(IMetaRepository metaRepository, ILogger<DeleteModel> logger, IMapper mapper)
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
                _logger.LogError(ex, "Erro ao carregar meta para exclusão");
                TempData["Error"] = "Erro ao carregar meta.";
                return RedirectToPage("Index");
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _metaRepository.DeleteAsync(Meta.Id);
                TempData["Success"] = "Meta excluída com sucesso!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir meta");
                TempData["Error"] = "Erro ao excluir meta.";
                return Page();
            }
        }
    }
}
