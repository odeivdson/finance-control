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
    public class MetasDeletePageModelTests
    {
        [Fact]
        public async Task OnGetAsync_MetaExiste_DeveCarregarViewModel()
        {
            var id = Guid.NewGuid();
            var meta = new MetaDto { Id = id, Descricao = "Meta", ValorAlvo = 100 };
            var mockRepo = new Mock<IMetaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(meta);
            var mockLogger = new Mock<ILogger<DeleteModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<MetaViewModel>(meta)).Returns(new MetaViewModel { Id = id });
            var pageModel = new DeleteModel(mockRepo.Object, mockLogger.Object, mockMapper.Object);

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
            var mockLogger = new Mock<ILogger<DeleteModel>>();
            var mockMapper = new Mock<IMapper>();
            var pageModel = new DeleteModel(mockRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }
    }
}
