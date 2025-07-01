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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tests
{
    public class TransacoesEditPageModelTests
    {
        [Fact]
        public async Task OnGetAsync_TransacaoExiste_DeveCarregarViewModelECategorias()
        {
            var id = Guid.NewGuid();
            var transacao = new TransacaoDto { Id = id, CategoriaId = Guid.NewGuid(), Descricao = "Teste" };
            var categorias = new List<CategoriaDto> { new CategoriaDto { Id = transacao.CategoriaId, Nome = "Categoria", Tipo = 1 } };
            var mockTransacaoRepo = new Mock<ITransacaoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<EditModel>>();
            var mockMapper = new Mock<IMapper>();
            mockTransacaoRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(transacao);
            mockCategoriaRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categorias);
            mockMapper.Setup(m => m.Map<TransacaoViewModel>(transacao)).Returns(new TransacaoViewModel { Id = id, CategoriaId = transacao.CategoriaId });
            var pageModel = new EditModel(mockTransacaoRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object);

            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());
            
            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<PageResult>(result);
            Assert.Equal(id, pageModel.Transacao.Id);
            Assert.NotNull(pageModel.Categorias);
        }

        [Fact]
        public async Task OnGetAsync_TransacaoNaoExiste_DeveRedirecionarParaIndexComErro()
        {
            var id = Guid.NewGuid();
            var mockTransacaoRepo = new Mock<ITransacaoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<EditModel>>();
            var mockMapper = new Mock<IMapper>();
            mockTransacaoRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TransacaoDto?)null);
            var pageModel = new EditModel(mockTransacaoRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }

        [Fact]
        public async Task OnPostAsync_ModelStateInvalido_DeveRetornarPageComErroECategorias()
        {
            var mockTransacaoRepo = new Mock<ITransacaoRepository>();
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<EditModel>>();
            var mockMapper = new Mock<IMapper>();
            var pageModel = new EditModel(mockTransacaoRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.ModelState.AddModelError("Descricao", "Obrigatório");
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());
            mockCategoriaRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<CategoriaDto> { new CategoriaDto { Id = Guid.NewGuid(), Nome = "Categoria", Tipo = 1 } });

            var result = await pageModel.OnPostAsync();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
            Assert.NotNull(pageModel.Categorias);
        }

        [Fact]
        public async Task OnPostAsync_Sucesso_DeveRedirecionarParaIndexComSuccess()
        {
            var mockTransacaoRepo = new Mock<ITransacaoRepository>();
            mockTransacaoRepo.Setup(r => r.UpdateAsync(It.IsAny<TransacaoDto>())).Returns(Task.CompletedTask);
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            mockCategoriaRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<CategoriaDto> { new CategoriaDto { Id = Guid.NewGuid(), Nome = "Categoria", Tipo = 1 } });
            var mockLogger = new Mock<ILogger<EditModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<TransacaoDto>(It.IsAny<TransacaoViewModel>())).Returns(new TransacaoDto { Id = Guid.NewGuid(), Descricao = "Teste", Valor = 100, CategoriaId = Guid.NewGuid(), Tipo = 1, Status = 1, Data = DateTime.Today });
            var pageModel = new EditModel(mockTransacaoRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Transacao = new TransacaoViewModel { Descricao = "Teste", Valor = 100, CategoriaId = Guid.NewGuid(), Tipo = 1, Status = 1, Data = DateTime.Today }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Success"]);
        }

        [Fact]
        public async Task OnPostAsync_RepositorioLancaExcecao_DeveRetornarPageComErroECategorias()
        {
            var mockTransacaoRepo = new Mock<ITransacaoRepository>();
            mockTransacaoRepo.Setup(r => r.UpdateAsync(It.IsAny<TransacaoDto>())).ThrowsAsync(new Exception("Erro"));
            var mockCategoriaRepo = new Mock<ICategoriaRepository>();
            mockCategoriaRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<CategoriaDto> { new CategoriaDto { Id = Guid.NewGuid(), Nome = "Categoria", Tipo = 1 } });
            var mockLogger = new Mock<ILogger<EditModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<TransacaoDto>(It.IsAny<TransacaoViewModel>())).Returns(new TransacaoDto { Id = Guid.NewGuid(), Descricao = "Teste", Valor = 100, CategoriaId = Guid.NewGuid(), Tipo = 1, Status = 1, Data = DateTime.Today });
            var pageModel = new EditModel(mockTransacaoRepo.Object, mockCategoriaRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Transacao = new TransacaoViewModel { Descricao = "Teste", Valor = 100, CategoriaId = Guid.NewGuid(), Tipo = 1, Status = 1, Data = DateTime.Today }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
            Assert.NotNull(pageModel.Categorias);
        }
    }
}
