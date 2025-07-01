using Xunit;
using Moq;
using Application.Services;
using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class ParcelaServiceTests
    {
        [Fact]
        public async Task GetAllAsync_DeveRetornarParcelas()
        {
            var parcelas = new List<ParcelaDto> { new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 10 } };
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(parcelas);
            var service = new ParcelaService(mockRepo.Object);

            var result = await service.GetAllAsync();

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
            var service = new ParcelaService(mockRepo.Object);

            var found = await service.GetByIdAsync(id);
            var notFound = await service.GetByIdAsync(Guid.NewGuid());

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
            var service = new ParcelaService(mockRepo.Object);

            var result = await service.GetByTransacaoIdAsync(transacaoId);

            Assert.All(result, p => Assert.Equal(transacaoId, p.TransacaoId));
        }

        [Fact]
        public async Task AddAsync_DeveChamarRepositorio()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            var dto = new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 10, DataVencimento = DateTime.Today.AddDays(1) };

            await service.AddAsync(dto);

            mockRepo.Verify(r => r.AddAsync(dto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DeveChamarRepositorio()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            var dto = new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 10, DataVencimento = DateTime.Today.AddDays(1) };

            await service.UpdateAsync(dto);

            mockRepo.Verify(r => r.UpdateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeveChamarRepositorio()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            var id = Guid.NewGuid();

            await service.DeleteAsync(id);

            mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task AddAsync_NullDto_DeveLancarArgumentNullException()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null));
        }

        [Fact]
        public async Task AddAsync_ValorInvalido_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            var dto = new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 0, DataVencimento = DateTime.Today.AddDays(1) };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_NumeroInvalido_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            var dto = new ParcelaDto { Id = Guid.NewGuid(), Numero = 0, Valor = 10, DataVencimento = DateTime.Today.AddDays(1) };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_DataVencimentoInvalida_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            var dto = new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 10, DataVencimento = default };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_NullDto_DeveLancarArgumentNullException()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_ValorInvalido_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            var dto = new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 0, DataVencimento = DateTime.Today.AddDays(1) };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_NumeroInvalido_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            var dto = new ParcelaDto { Id = Guid.NewGuid(), Numero = 0, Valor = 10, DataVencimento = DateTime.Today.AddDays(1) };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_DataVencimentoInvalida_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            var dto = new ParcelaDto { Id = Guid.NewGuid(), Numero = 1, Valor = 10, DataVencimento = default };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(dto));
        }

        [Fact]
        public async Task DeleteAsync_IdVazio_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            var service = new ParcelaService(mockRepo.Object);
            await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteAsync(Guid.Empty));
        }

        [Fact]
        public async Task GetAllAsync_RepositorioRetornaVazio_DeveRetornarListaVazia()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ParcelaDto>());
            var service = new ParcelaService(mockRepo.Object);

            var result = await service.GetAllAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_RepositorioRetornaNull_DeveRetornarNull()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ParcelaDto?)null);
            var service = new ParcelaService(mockRepo.Object);

            var result = await service.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByTransacaoIdAsync_RepositorioRetornaVazio_DeveRetornarListaVazia()
        {
            var mockRepo = new Mock<IParcelaRepository>();
            mockRepo.Setup(r => r.GetByTransacaoIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<ParcelaDto>());
            var service = new ParcelaService(mockRepo.Object);

            var result = await service.GetByTransacaoIdAsync(Guid.NewGuid());

            Assert.Empty(result);
        }
    }
}
