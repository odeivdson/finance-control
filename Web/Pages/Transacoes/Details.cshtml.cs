using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace Web.Pages.Transacoes
{
    public class DetailsModel : PageModel
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<DetailsModel> _logger;
        private readonly IMapper _mapper;

        public DetailsModel(
            ITransacaoRepository transacaoRepository,
            ICategoriaRepository categoriaRepository,
            ILogger<DetailsModel> logger,
            IMapper mapper)
        {
            _transacaoRepository = transacaoRepository;
            _categoriaRepository = categoriaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public TransacaoViewModel? Transacao { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var transacao = await _transacaoRepository.GetByIdAsync(id);
                if (transacao == null)
                {
                    TempData["Error"] = "Transação não encontrada.";
                    return RedirectToPage("Index");
                }
                Transacao = _mapper.Map<TransacaoViewModel>(transacao);
                var categoria = await _categoriaRepository.GetByIdAsync(transacao.CategoriaId);
                CategoriaNome = categoria?.Nome ?? string.Empty;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar detalhes da transação");
                TempData["Error"] = "Erro ao carregar detalhes da transação.";
                return RedirectToPage("Index");
            }
        }
    }
}