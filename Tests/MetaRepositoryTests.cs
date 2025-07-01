using Xunit;
using Moq;
using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class MetaRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_DeveRetornarMetas()
        {
            var metas = new List<MetaDto> { new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100 } };
            var mockRepo = new Mock<IMetaRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(metas);

            var result = await mockRepo.Object.GetAllAsync();

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

            var found = await mockRepo.Object.GetByIdAsync(id);
            var notFound = await mockRepo.Object.GetByIdAsync(Guid.NewGuid());

            Assert.NotNull(found);
            Assert.Null(notFound);
        }

        [Fact]
        public async Task AddAsync_DeveSerChamado()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100 };
            await mockRepo.Object.AddAsync(dto);
            mockRepo.Verify(r => r.AddAsync(dto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DeveSerChamado()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100 };
            await mockRepo.Object.UpdateAsync(dto);
            mockRepo.Verify(r => r.UpdateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeveSerChamado()
        {
            var mockRepo = new Mock<IMetaRepository>();
            var id = Guid.NewGuid();
            await mockRepo.Object.DeleteAsync(id);
            mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }
}
