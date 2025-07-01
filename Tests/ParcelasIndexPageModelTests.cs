using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Web.Pages.Parcelas;

namespace Tests
{
    public class ParcelasIndexPageModelTests
    {
        [Fact]
        public async Task OnGetAsync_RepositorioLancaExcecao_DeveSetarTempDataError()
        {
            var transacaoId = Guid.NewGuid();
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.GetByTransacaoIdAsync(transacaoId)).ThrowsAsync(new Exception("Erro de banco"));
            var mockLogger = new Mock<ILogger<IndexModel>>();
            var pageModel = new IndexModel(mockRepo.Object, mockLogger.Object)
            {
                TransacaoId = transacaoId,
                PageSize = 10,
                CurrentPage = 1
            };
            pageModel.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            await pageModel.OnGetAsync();

            Assert.NotNull(pageModel.TempData["Error"]);
            Assert.Contains("Erro de banco", pageModel.TempData["Error"].ToString());
        }
    }
}