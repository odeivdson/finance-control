using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Web.ViewModels;
using System;
using System.Threading.Tasks;

namespace Web.Pages.Transacoes
{
    public class CreateModel : PageModel
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ITransacaoRepository transacaoRepository, ICategoriaRepository categoriaRepository, ILogger<CreateModel> logger)
        {
            _transacaoRepository = transacaoRepository;
            _categoriaRepository = categoriaRepository;
            _logger = logger;
        }

        [BindProperty]
        public TransacaoViewModel Transacao { get; set; } = new();
        public SelectList Categorias { get; set; } = null!;

        public async Task OnGetAsync()
        {
            try
            {
                var categorias = await _categoriaRepository.GetAllAsync();
                Categorias = new SelectList(categorias, "Id", "Nome");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar categorias");
                TempData["Error"] = "Erro ao carregar categorias.";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Preencha todos os campos obrigatórios corretamente.";
                try
                {
                    var categorias = await _categoriaRepository.GetAllAsync();
                    Categorias = new SelectList(categorias, "Id", "Nome");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao carregar categorias");
                    TempData["Error"] = "Erro ao carregar categorias.";
                }
                return Page();
            }

            try
            {
                var dto = new TransacaoDto
                {
                    Id = Guid.NewGuid(),
                    Descricao = Transacao.Descricao,
                    Data = Transacao.Data,
                    Valor = Transacao.Valor,
                    CategoriaId = Transacao.CategoriaId,
                    Tipo = Transacao.Tipo,
                    Status = Transacao.Status
                };
                await _transacaoRepository.AddAsync(dto);
                TempData["Success"] = "Transação criada com sucesso!";
                return RedirectToPage("/Transacoes/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar transação");
                TempData["Error"] = "Erro ao criar transação.";
                try
                {
                    var categorias = await _categoriaRepository.GetAllAsync();
                    Categorias = new SelectList(categorias, "Id", "Nome");
                }
                catch (Exception ex2)
                {
                    _logger.LogError(ex2, "Erro ao carregar categorias");
                    TempData["Error"] = "Erro ao carregar categorias.";
                }
                return Page();
            }
        }
    }
}