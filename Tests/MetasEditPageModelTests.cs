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
    public class MetasEditPageModelTests
    {
        [Fact]
        public async Task OnGetAsync_MetaExiste_DeveCarregarViewModel()
        {
            var id = Guid.NewGuid();
            var meta = new MetaDto { Id = id, Descricao = "Meta", ValorAlvo = 100 };
            var mockRepo = new Mock<IMetaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(meta);
            var mockLogger = new Mock<ILogger<EditModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<MetaViewModel>(meta)).Returns(new MetaViewModel { Id = id });
            var pageModel = new EditModel(mockRepo.Object, mockLogger.Object, mockMapper.Object);

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<PageResult>(result);
            Assert.Equal(id, pageModel.Meta.Id);
        }

        [Fact]
        public async Task OnGetAsync_MetaNaoExiste_DeveRedirecionarParaIndexComErro()
        {
            var id = Guid.NewGuid();
            var mockRepo = new Mock<IMetaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((MetaDto?)null);
            var mockLogger = new Mock<ILogger<EditModel>>();
            var mockMapper = new Mock<IMapper>();
            var pageModel = new EditModel(mockRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }

        [Fact]
        public async Task OnPostAsync_ModelStateInvalido_DeveRetornarPageComErro()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var mockLogger = new Mock<ILogger<EditModel>>();
            var mockMapper = new Mock<IMapper>();
            var pageModel = new EditModel(mockRepo.Object, mockLogger.Object, mockMapper.Object);
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
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<MetaDto>())).Returns(Task.CompletedTask);
            var mockLogger = new Mock<ILogger<EditModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<MetaDto>(It.IsAny<MetaViewModel>())).Returns(new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100 });
            var pageModel = new EditModel(mockRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Meta = new MetaViewModel { Descricao = "Meta", ValorAlvo = 100 }
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
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<MetaDto>())).ThrowsAsync(new Exception("Erro"));
            var mockLogger = new Mock<ILogger<EditModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<MetaDto>(It.IsAny<MetaViewModel>())).Returns(new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100 });
            var pageModel = new EditModel(mockRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Meta = new MetaViewModel { Descricao = "Meta", ValorAlvo = 100 }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }
    }
}
