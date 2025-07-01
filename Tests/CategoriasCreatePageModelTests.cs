using Xunit;
using Moq;
using Web.Pages.Categorias;
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
    public class CategoriasCreatePageModelTests
    {
        [Fact]
        public async Task OnPostAsync_ModelStateInvalido_DeveRetornarPageComErro()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var mockLogger = new Mock<ILogger<CreateModel>>();
            var mockMapper = new Mock<IMapper>();
            var pageModel = new CreateModel(mockRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.ModelState.AddModelError("Nome", "Obrigatório");
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }

        [Fact]
        public async Task OnPostAsync_Sucesso_DeveRedirecionarParaIndexComSuccess()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<CategoriaDto>())).Returns(Task.CompletedTask);
            var mockLogger = new Mock<ILogger<CreateModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<CategoriaDto>(It.IsAny<CategoriaViewModel>())).Returns(new CategoriaDto { Id = Guid.NewGuid(), Nome = "Teste", Tipo = 1 });
            var pageModel = new CreateModel(mockRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Categoria = new CategoriaViewModel { Nome = "Teste", Tipo = 1 }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Success"]);
        }

        [Fact]
        public async Task OnPostAsync_RepositorioLancaExcecao_DeveRetornarPageComErro()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<CategoriaDto>())).ThrowsAsync(new Exception("Erro"));
            var mockLogger = new Mock<ILogger<CreateModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<CategoriaDto>(It.IsAny<CategoriaViewModel>())).Returns(new CategoriaDto { Id = Guid.NewGuid(), Nome = "Teste", Tipo = 1 });
            var pageModel = new CreateModel(mockRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Categoria = new CategoriaViewModel { Nome = "Teste", Tipo = 1 }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }
    }
}
