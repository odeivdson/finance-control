using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Web.Pages.Parcelas
{
    public class IndexModel : PageModel
    {
        private readonly IParcelaRepository _parcelaRepository;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(IParcelaRepository parcelaRepository, ILogger<IndexModel> logger)
        {
            _parcelaRepository = parcelaRepository;
            _logger = logger;
        }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<ParcelaDto> Parcelas { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public Guid? TransacaoId { get; set; }
        public async Task OnGetAsync()
        {
            try
            {
                if (TransacaoId.HasValue)
                {
                    var allParcelas = (await _parcelaRepository.GetByTransacaoIdAsync(TransacaoId.Value)).ToList();
                    // Atualização automática: parcelas vencidas e pendentes são marcadas como quitadas
                    var hoje = DateTime.Today;
                    var alteradas = false;
                    foreach (var parcela in allParcelas.Where(p => p.Status == 1 && p.DataVencimento.Date <= hoje))
                    {
                        parcela.Status = 2; // Quitado
                        await _parcelaRepository.UpdateAsync(parcela);
                        alteradas = true;
                    }
                    if (alteradas)
                    {
                        allParcelas = (await _parcelaRepository.GetByTransacaoIdAsync(TransacaoId.Value)).ToList();
                        TempData["Success"] = "Parcelas vencidas foram marcadas como quitadas automaticamente.";
                    }
                    TotalCount = allParcelas.Count;
                    TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
                    Parcelas = allParcelas
                        .OrderByDescending(p => p.DataVencimento)
                        .Skip((CurrentPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar parcelas");
                TempData["Error"] = $"Ocorreu um erro ao carregar as parcelas: {ex.Message}";
            }
        }
        public string GetQueryString()
        {
            return TransacaoId.HasValue ? $"&TransacaoId={TransacaoId}" : string.Empty;
        }
    }
}
