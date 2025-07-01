using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace Web.Pages.Transacoes
{
    public class EditModel : PageModel
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<EditModel> _logger;
        private readonly IMapper _mapper;

        public EditModel(ITransacaoRepository transacaoRepository, ICategoriaRepository categoriaRepository, ILogger<EditModel> logger, IMapper mapper)
        {
            _transacaoRepository = transacaoRepository;
            _categoriaRepository = categoriaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [BindProperty]
        public TransacaoViewModel Transacao { get; set; } = new();
        public SelectList Categorias { get; set; } = null!;

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
                var categorias = await _categoriaRepository.GetAllAsync();
                Categorias = new SelectList(categorias, "Id", "Nome");
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar transação para edição");
                TempData["Error"] = "Erro ao carregar transação.";
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                var categorias = await _categoriaRepository.GetAllAsync();
                Categorias = new SelectList(categorias, "Id", "Nome");
                return Page();
            }
            try
            {
                var dto = _mapper.Map<TransacaoDto>(Transacao);
                await _transacaoRepository.UpdateAsync(dto);
                TempData["Success"] = "Transação editada com sucesso!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar transação");
                TempData["Error"] = "Erro ao editar transação.";
                var categorias = await _categoriaRepository.GetAllAsync();
                Categorias = new SelectList(categorias, "Id", "Nome");
                return Page();
            }
        }
    }
}