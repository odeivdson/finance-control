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
    public class ParcelasCreatePageModelTests
    {
        [Fact]
        public async Task OnPostAsync_ModelStateInvalido_DeveRetornarPageComErro()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var mockLogger = new Mock<ILogger<CreateModel>>();
            var mockMapper = new Mock<IMapper>();
            var pageModel = new CreateModel(mockRepo.Object, mockLogger.Object, mockMapper.Object);
            pageModel.ModelState.AddModelError("Valor", "Obrigatório");
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }

        [Fact]
        public async Task OnPostAsync_Sucesso_DeveRedirecionarParaIndexComSuccess()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<ParcelaDto>())).Returns(Task.CompletedTask);
            var mockLogger = new Mock<ILogger<CreateModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<ParcelaDto>(It.IsAny<ParcelaViewModel>())).Returns(new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 10, DataVencimento = DateTime.Today, TransacaoId = Guid.NewGuid() });
            var pageModel = new CreateModel(mockRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Parcela = new ParcelaViewModel { Numero = 1, Valor = 10, DataVencimento = DateTime.Today, TransacaoId = Guid.NewGuid() }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<RedirectToPageResult>(result);
            Assert.NotNull(pageModel.TempData["Success"]);
        }

        [Fact]
        public async Task OnPostAsync_RepositorioLancaExcecao_DeveRetornarPageComErro()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<ParcelaDto>())).ThrowsAsync(new Exception("Erro"));
            var mockLogger = new Mock<ILogger<CreateModel>>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<ParcelaDto>(It.IsAny<ParcelaViewModel>())).Returns(new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 10, DataVencimento = DateTime.Today, TransacaoId = Guid.NewGuid() });
            var pageModel = new CreateModel(mockRepo.Object, mockLogger.Object, mockMapper.Object)
            {
                Parcela = new ParcelaViewModel { Numero = 1, Valor = 10, DataVencimento = DateTime.Today, TransacaoId = Guid.NewGuid() }
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await pageModel.OnPostAsync();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TempData["Error"]);
        }
    }
}
