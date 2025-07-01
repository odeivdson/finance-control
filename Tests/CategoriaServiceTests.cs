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
    public class CategoriaServiceTests
    {
        [Fact]
        public async Task GetAllAsync_DeveRetornarCategorias()
        {
            var categorias = new List<CategoriaDto> { new CategoriaDto { Id = Guid.NewGuid(), Nome = "Teste", Tipo = 1 } };
            var mockRepo = new Mock<ICategoriaRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categorias);
            var service = new CategoriaService(mockRepo.Object);

            var result = await service.GetAllAsync();

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
            var service = new CategoriaService(mockRepo.Object);

            var found = await service.GetByIdAsync(id);
            var notFound = await service.GetByIdAsync(Guid.NewGuid());

            Assert.NotNull(found);
            Assert.Null(notFound);
        }

        [Fact]
        public async Task AddAsync_DeveChamarRepositorio()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var service = new CategoriaService(mockRepo.Object);
            var dto = new CategoriaDto { Id = Guid.NewGuid(), Nome = "Teste", Tipo = 1 };

            await service.AddAsync(dto);

            mockRepo.Verify(r => r.AddAsync(dto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DeveChamarRepositorio()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var service = new CategoriaService(mockRepo.Object);
            var dto = new CategoriaDto { Id = Guid.NewGuid(), Nome = "Teste", Tipo = 1 };

            await service.UpdateAsync(dto);

            mockRepo.Verify(r => r.UpdateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeveChamarRepositorio()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var service = new CategoriaService(mockRepo.Object);
            var id = Guid.NewGuid();

            await service.DeleteAsync(id);

            mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task AddAsync_NullDto_DeveLancarArgumentNullException()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var service = new CategoriaService(mockRepo.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null));
        }

        [Fact]
        public async Task AddAsync_NomeVazio_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var service = new CategoriaService(mockRepo.Object);
            var dto = new CategoriaDto { Id = Guid.NewGuid(), Nome = "", Tipo = 1 };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_TipoInvalido_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var service = new CategoriaService(mockRepo.Object);
            var dto = new CategoriaDto { Id = Guid.NewGuid(), Nome = "Teste", Tipo = 0 };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_NullDto_DeveLancarArgumentNullException()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var service = new CategoriaService(mockRepo.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_NomeVazio_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var service = new CategoriaService(mockRepo.Object);
            var dto = new CategoriaDto { Id = Guid.NewGuid(), Nome = "", Tipo = 1 };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_TipoInvalido_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var service = new CategoriaService(mockRepo.Object);
            var dto = new CategoriaDto { Id = Guid.NewGuid(), Nome = "Teste", Tipo = 0 };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(dto));
        }

        [Fact]
        public async Task DeleteAsync_IdVazio_DeveLancarArgumentException()
        {
            var mockRepo = new Mock<ICategoriaRepository>();
            var service = new CategoriaService(mockRepo.Object);
            await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteAsync(Guid.Empty));
        }
    }
}
