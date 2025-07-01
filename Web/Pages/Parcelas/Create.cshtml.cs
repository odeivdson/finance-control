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
    public class CreateModel : PageModel
    {
        private readonly IParcelaRepository _parcelaRepository;
        private readonly ILogger<CreateModel> _logger;
        private readonly IMapper _mapper;
        public CreateModel(IParcelaRepository parcelaRepository, ILogger<CreateModel> logger, IMapper mapper)
        {
            _parcelaRepository = parcelaRepository;
            _logger = logger;
            _mapper = mapper;
        }
        [BindProperty]
        public ParcelaViewModel Parcela { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public Guid? TransacaoId { get; set; }
        public void OnGet()
        {
            if (TransacaoId.HasValue)
                Parcela.TransacaoId = TransacaoId.Value;
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
                Parcela.Id = Guid.NewGuid();
                var dto = _mapper.Map<ParcelaDto>(Parcela);
                await _parcelaRepository.AddAsync(dto);
                TempData["Success"] = "Parcela criada com sucesso!";
                return RedirectToPage("Index", new { transacaoId = Parcela.TransacaoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar parcela");
                TempData["Error"] = "Erro ao criar parcela.";
                return Page();
            }
        }
    }
}
