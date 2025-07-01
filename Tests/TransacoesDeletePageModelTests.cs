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
    public class TransacoesDeletePageModelTests
    {
        [Fact]
        public async Task OnGetAsync_TransacaoExiste_DeveCarregarViewModelECategoria()
        {
            var id = Guid.NewGuid();
            var transacao = new TransacaoDto { Id = id, CategoriaId = Guid.NewGuid(), Descricao = "Teste" };
            var categoria = new CategoriaDto { Id = transacao.CategoriaId, Nome = "Categoria" };
            var mockTransacaoRepo = new Mock<ITransacaoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<DeleteModel>>();
            var mockMapper = new Mock<IMapper>();
            mockTransacaoRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(transacao);
            mockCategoriaRepo.Setup(r => r.GetByIdAsync(transacao.CategoriaId)).ReturnsAsync(categoria);
            mockMapper.Setup(m => m.Map<TransacaoViewModel>(transacao)).Returns(new TransacaoViewModel { Id = id, CategoriaId = transacao.CategoriaId });
            var pageModel = new DeleteModel(mockTransacaoRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object);

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<PageResult>(result);
            Assert.Equal(id, pageModel.Transacao.Id);
            Assert.Equal("Categoria", pageModel.CategoriaNome);
        }

        [Fact]
        public async Task OnGetAsync_TransacaoNaoExiste_DeveRedirecionarParaIndexComErro()
        {
            var id = Guid.NewGuid();
            var mockTransacaoRepo = new Mock<ITransacaoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<DeleteModel>>();
            var mockMapper = new Mock<IMapper>();
            mockTransacaoRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TransacaoDto?)null);
            var pageModel = new DeleteModel(mockTransacaoRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }

        [Fact]
        public async Task OnGetAsync_RepositorioLancaExcecao_DeveRedirecionarParaIndexComErro()
        {
            var id = Guid.NewGuid();
            var mockTransacaoRepo = new Mock<ITransacaoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<DeleteModel>>();
            var mockMapper = new Mock<IMapper>();
            mockTransacaoRepo.Setup(r => r.GetByIdAsync(id)).ThrowsAsync(new Exception("Erro"));
            var pageModel = new DeleteModel(mockTransacaoRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }

        [Fact]
        public async Task OnPostAsync_TransacaoExiste_DeveExcluirTransacaoERedirecionarParaIndex()
        {
            var id = Guid.NewGuid();
            var transacao = new TransacaoDto { Id = id, CategoriaId = Guid.NewGuid(), Descricao = "Teste" };
            var categoria = new CategoriaDto { Id = transacao.CategoriaId, Nome = "Categoria" };
            var mockTransacaoRepo = new Mock<ITransacaoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<DeleteModel>>();
            var mockMapper = new Mock<IMapper>();
            mockTransacaoRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(transacao);
            mockCategoriaRepo.Setup(r => r.GetByIdAsync(transacao.CategoriaId)).ReturnsAsync(categoria);
            mockTransacaoRepo.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);
            mockMapper.Setup(m => m.Map<TransacaoViewModel>(transacao)).Returns(new TransacaoViewModel { Id = id, CategoriaId = transacao.CategoriaId });
            var pageModel = new DeleteModel(mockTransacaoRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Transacao = new TransacaoViewModel { Id = id, CategoriaId = transacao.CategoriaId }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<RedirectToPageResult>(result);
            mockTransacaoRepo.Verify(r => r.DeleteAsync(id), Times.Once);
            Assert.NotNull(pageModel.TempData["Success"]);
        }

        [Fact]
        public async Task OnPostAsync_RepositorioLancaExcecao_DeveRetornarPageComErro()
        {
            var id = Guid.NewGuid();
            var mockTransacaoRepo = new Mock<ITransacaoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<DeleteModel>>();
            var mockMapper = new Mock<IMapper>();
            mockTransacaoRepo.Setup(r => r.DeleteAsync(id)).ThrowsAsync(new Exception("Erro"));
            var pageModel = new DeleteModel(mockTransacaoRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Transacao = new TransacaoViewModel { Id = id, CategoriaId = Guid.NewGuid() }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }
    }
}