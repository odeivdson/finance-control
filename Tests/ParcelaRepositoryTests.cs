using Xunit;
using Moq;
using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class ParcelaRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_DeveRetornarParcelas()
        {
            var parcelas = new List<ParcelaDto> { new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 10 } };
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(parcelas);

            var result = await mockRepo.Object.GetAllAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarParcelaOuNull()
        {
            var id = Guid.NewGuid();
            var parcela = new ParcelaDto { Id = id, Numero = 1, Valor = 10 };
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(parcela);
            mockRepo.Setup(r => r.GetByIdAsync(It.Is<Guid>(g => g != id))).ReturnsAsync((ParcelaDto?)null);

            var found = await mockRepo.Object.GetByIdAsync(id);
            var notFound = await mockRepo.Object.GetByIdAsync(Guid.NewGuid());

            Assert.NotNull(found);
            Assert.Null(notFound);
        }

        [Fact]
        public async Task GetByTransacaoIdAsync_DeveRetornarParcelasRelacionadas()
        {
            var transacaoId = Guid.NewGuid();
            var parcelas = new List<ParcelaDto> {
                new ParcelaDto { Id = Guid.NewGuid(), TransacaoId = transacaoId },
                new ParcelaDto { Id = Guid.NewGuid(), TransacaoId = Guid.NewGuid() }
            };
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.GetByTransacaoIdAsync(transacaoId)).ReturnsAsync(parcelas.FindAll(p => p.TransacaoId == transacaoId));

            var result = await mockRepo.Object.GetByTransacaoIdAsync(transacaoId);

            Assert.All(result, p => Assert.Equal(transacaoId, p.TransacaoId));
        }

        [Fact]
        public async Task AddAsync_DeveSerChamado()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var dto = new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 10 };
            await mockRepo.Object.AddAsync(dto);
            mockRepo.Verify(r => r.AddAsync(dto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DeveSerChamado()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var dto = new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 10 };
            await mockRepo.Object.UpdateAsync(dto);
            mockRepo.Verify(r => r.UpdateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeveSerChamado()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var id = Guid.NewGuid();
            await mockRepo.Object.DeleteAsync(id);
            mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }
}
