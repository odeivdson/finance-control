using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace Web.Pages.Parcelas
{
    public class DetailsModel : PageModel
    {
        private readonly IParcelaRepository _parcelaRepository;
        private readonly ILogger<DetailsModel> _logger;
        private readonly IMapper _mapper;
        public DetailsModel(IParcelaRepository parcelaRepository, ILogger<DetailsModel> logger, IMapper mapper)
        {
            _parcelaRepository = parcelaRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public ParcelaViewModel? Parcela { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var parcela = await _parcelaRepository.GetByIdAsync(id);
                if (parcela == null)
                {
                    TempData["Error"] = "Parcela não encontrada.";
                    return RedirectToPage("Index");
                }
                Parcela = _mapper.Map<ParcelaViewModel>(parcela);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar detalhes da parcela");
                TempData["Error"] = "Erro ao carregar detalhes da parcela.";
                return RedirectToPage("Index");
            }
        }
    }
}
