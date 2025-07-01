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
    }
}