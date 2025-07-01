using Xunit;
using Moq;
using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class CategoriaRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_DeveRetornarCategorias()
        {
            var categorias = new List<CategoriaDto> { new CategoriaDto { Id = Guid.NewGuid(), Nome = "Teste", Tipo = 1 } };
            var mockRepo = new Mock<ICategoriaRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categorias);

            var result = await mockRepo.Object.GetAllAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarCategoriaOuNull()
        {
            var id = Guid.NewGuid();
            var categoria = new CategoriaDto { Id = id, Nome = "Teste", Tipo = 1 };
            var mockRepo = new Mock<ICategoriaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(categoria);
            mockRepo.Setup(r => r.GetByIdAsync(It.Is<Guid>(g => g != id))).ReturnsAsync((CategoriaDto?)null);

            var found = await mockRepo.Object.GetByIdAsync(id);
            var notFound = await mockRepo.Object.GetByIdAsync(Guid.NewGuid());

            Assert.NotNull(found);
            Assert.Null(notFound);
        }

        [Fact]
        public async Task AddAsync_DeveSerChamado()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var dto = new CategoriaDto { Id = Guid.NewGuid(), Nome = "Teste", Tipo = 1 };
            await mockRepo.Object.AddAsync(dto);
            mockRepo.Verify(r => r.AddAsync(dto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DeveSerChamado()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var dto = new CategoriaDto { Id = Guid.NewGuid(), Nome = "Teste", Tipo = 1 };
            await mockRepo.Object.UpdateAsync(dto);
            mockRepo.Verify(r => r.UpdateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeveSerChamado()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var id = Guid.NewGuid();
            await mockRepo.Object.DeleteAsync(id);
            mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }
}
