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
    public class MetaServiceTests
    {
        [Fact]
        public async Task GetAllAsync_DeveRetornarMetas()
        {
            var metas = new List<MetaDto> { new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100 } };
            var mockRepo = new Mock<IMetaRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(metas);
            var service = new MetaService(mockRepo.Object);

            var result = await service.GetAllAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarMetaOuNull()
        {
            var id = Guid.NewGuid();
            var meta = new MetaDto { Id = id, Descricao = "Meta", ValorAlvo = 100 };
            var mockRepo = new Mock<IMetaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(meta);
            mockRepo.Setup(r => r.GetByIdAsync(It.Is<Guid>(g => g != id))).ReturnsAsync((MetaDto?)null);
            var service = new MetaService(mockRepo.Object);

            var found = await service.GetByIdAsync(id);
            var notFound = await service.GetByIdAsync(Guid.NewGuid());

            Assert.NotNull(found);
            Assert.Null(notFound);
        }

        [Fact]
        public async Task AddAsync_DeveChamarRepositorio()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100, DataLimite = DateTime.Today.AddDays(1) };

            await service.AddAsync(dto);

            mockRepo.Verify(r => r.AddAsync(dto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DeveChamarRepositorio()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100, DataLimite = DateTime.Today.AddDays(1) };

            await service.UpdateAsync(dto);

            mockRepo.Verify(r => r.UpdateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeveChamarRepositorio()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            var id = Guid.NewGuid();

            await service.DeleteAsync(id);

            mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task AddAsync_NullDto_DeveLancarArgumentNullException()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null));
        }

        [Fact]
        public async Task AddAsync_DescricaoVazia_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "", ValorAlvo = 100, DataLimite = DateTime.Today.AddDays(1) };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ValorAlvoInvalido_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 0, DataLimite = DateTime.Today.AddDays(1) };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_DataLimiteInvalida_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100, DataLimite = default };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_NullDto_DeveLancarArgumentNullException()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_DescricaoVazia_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "", ValorAlvo = 100, DataLimite = DateTime.Today.AddDays(1) };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ValorAlvoInvalido_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 0, DataLimite = DateTime.Today.AddDays(1) };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_DataLimiteInvalida_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100, DataLimite = default };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(dto));
        }

        [Fact]
        public async Task DeleteAsync_IdVazio_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var service = new MetaService(mockRepo.Object);
            await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteAsync(Guid.Empty));
        }
    }
}
