using Xunit;
using Moq;
using Web.Pages.Transacoes;
using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;
using AutoMapper;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tests
{
    public class TransacoesDetailsPageModelTests
    {
        [Fact]
        public async Task OnGetAsync_TransacaoExiste_DeveCarregarViewModel()
        {
            var id = Guid.NewGuid();
            var transacao = new TransacaoDto { Id = id, CategoriaId = Guid.NewGuid(), Descricao = "Teste" };
            var categoria = new CategoriaDto { Id = transacao.CategoriaId, Nome = "Categoria" };
            var mockRepo = new Mock<ITransacaoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<DetailsModel>>();
            var mockMapper = new Mock<IMapper>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(transacao);
            mockCategoriaRepo.Setup(r => r.GetByIdAsync(transacao.CategoriaId)).ReturnsAsync(categoria);
            mockMapper.Setup(m => m.Map<TransacaoViewModel>(transacao)).Returns(new TransacaoViewModel { Id = id });
            var pageModel = new DetailsModel(mockRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object);

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<PageResult>(result);
            Assert.Equal(id, pageModel.Transacao.Id);
        }

        [Fact]
        public async Task OnGetAsync_TransacaoNaoExiste_DeveRedirecionarParaIndexComErro()
        {
            var id = Guid.NewGuid();
            var mockRepo = new Mock<ITransacaoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<DetailsModel>>();
            var mockMapper = new Mock<IMapper>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TransacaoDto?)null);
            var pageModel = new DetailsModel(mockRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }
    }
}
