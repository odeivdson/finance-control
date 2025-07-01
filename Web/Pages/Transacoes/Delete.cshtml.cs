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
    public class DeleteModel : PageModel
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IMapper _mapper;

        public DeleteModel(ITransacaoRepository transacaoRepository, ICategoriaRepository categoriaRepository, ILogger<DeleteModel> logger, IMapper mapper)
        {
            _transacaoRepository = transacaoRepository;
            _categoriaRepository = categoriaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [BindProperty]
        public TransacaoViewModel Transacao { get; set; } = new();
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
                _logger.LogError(ex, "Erro ao carregar transação para exclusão");
                TempData["Error"] = "Erro ao carregar transação.";
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _transacaoRepository.DeleteAsync(Transacao.Id);
                TempData["Success"] = "Transação excluída com sucesso!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir transação");
                TempData["Error"] = "Erro ao excluir transação.";
                return Page();
            }
        }
    }
}