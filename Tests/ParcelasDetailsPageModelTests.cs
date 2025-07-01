using Xunit;
using Moq;
using Web.Pages.Parcelas;
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
    public class ParcelasDetailsPageModelTests
    {
        [Fact]
        public async Task OnGetAsync_ParcelaExiste_DeveCarregarViewModel()
        {
            var id = Guid.NewGuid();
            var parcela = new ParcelaDto { Id = id, Numero = 1, Valor = 10 };
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(parcela);
            var mockLogger = new Mock<ILogger<DetailsModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<ParcelaViewModel>(parcela)).Returns(new ParcelaViewModel { Id = id });
            var pageModel = new DetailsModel(mockRepo.Object, mockLogger.Object, mockMapper.Object);

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<PageResult>(result);
            Assert.Equal(id, pageModel.Parcela.Id);
        }

        [Fact]
        public async Task OnGetAsync_ParcelaNaoExiste_DeveRedirecionarParaIndexComErro()
        {
            var id = Guid.NewGuid();
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ParcelaDto?)null);
            var mockLogger = new Mock<ILogger<DetailsModel>>();
            var mockMapper = new Mock<IMapper>();
            var pageModel = new DetailsModel(mockRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnGetAsync(id);

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }
    }
}
