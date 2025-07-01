using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace Web.Pages.Parcelas
{
    public class DeleteModel : PageModel
    {
        private readonly IParcelaRepository _parcelaRepository;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IMapper _mapper;
        public DeleteModel(IParcelaRepository parcelaRepository, ILogger<DeleteModel> logger, IMapper mapper)
        {
            _parcelaRepository = parcelaRepository;
            _logger = logger;
            _mapper = mapper;
        }
        [BindProperty]
        public ParcelaViewModel Parcela { get; set; } = new();
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
                _logger.LogError(ex, "Erro ao carregar parcela para exclusão");
                TempData["Error"] = "Erro ao carregar parcela.";
                return RedirectToPage("Index");
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _parcelaRepository.DeleteAsync(Parcela.Id);
                TempData["Success"] = "Parcela excluída com sucesso!";
                return RedirectToPage("Index", new { transacaoId = Parcela.TransacaoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir parcela");
                TempData["Error"] = "Erro ao excluir parcela.";
                return Page();
            }
        }
    }
}
