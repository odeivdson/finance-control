using Xunit;
using Moq;
using Web.Pages.Metas;
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
    public class MetasCreatePageModelTests
    {
        [Fact]
        public async Task OnPostAsync_ModelStateInvalido_DeveRetornarPageComErro()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var mockLogger = new Mock<ILogger<CreateModel>>();
            var mockMapper = new Mock<IMapper>();
            var pageModel = new CreateModel(mockRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.ModelState.AddModelError("Descricao", "Obrigatório");
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }

        [Fact]
        public async Task OnPostAsync_Sucesso_DeveRedirecionarParaIndexComSuccess()
        {
            var mockRepo = new Mock<IMetaRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<MetaDto>())).Returns(Task.CompletedTask);
            var mockLogger = new Mock<ILogger<CreateModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<MetaDto>(It.IsAny<MetaViewModel>())).Returns(new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100, DataLimite = DateTime.Today.AddDays(1) });
            var pageModel = new CreateModel(mockRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Meta = new MetaViewModel { Descricao = "Meta", ValorAlvo = 100, DataLimite = DateTime.Today.AddDays(1) }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Success"]);
        }

        [Fact]
        public async Task OnPostAsync_RepositorioLancaExcecao_DeveRetornarPageComErro()
        {
            var mockRepo = new Mock<IMetaRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<MetaDto>())).ThrowsAsync(new Exception("Erro"));
            var mockLogger = new Mock<ILogger<CreateModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<MetaDto>(It.IsAny<MetaViewModel>())).Returns(new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100, DataLimite = DateTime.Today.AddDays(1) });
            var pageModel = new CreateModel(mockRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Meta = new MetaViewModel { Descricao = "Meta", ValorAlvo = 100, DataLimite = DateTime.Today.AddDays(1) }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }
    }
}
