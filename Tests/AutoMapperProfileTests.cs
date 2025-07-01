using Xunit;
using AutoMapper;
using Web;
using Application.Models;
using Web.ViewModels;
using System;

namespace Tests
{
    public class AutoMapperProfileTests
    {
        [Fact]
        public void AutoMapper_TransacaoDtoParaViewModel_DeveMapearCorretamente()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            var mapper = config.CreateMapper();
            var dto = new TransacaoDto { Id = Guid.NewGuid(), Descricao = "Teste", Valor = 100, CategoriaId = Guid.NewGuid(), Tipo = 1, Status = 1, Data = DateTime.Today };
            var vm = mapper.Map<TransacaoViewModel>(dto);
            Assert.Equal(dto.Id, vm.Id);
            Assert.Equal(dto.Descricao, vm.Descricao);
            Assert.Equal(dto.Valor, vm.Valor);
            Assert.Equal(dto.CategoriaId, vm.CategoriaId);
            Assert.Equal(dto.Tipo, vm.Tipo);
            Assert.Equal(dto.Status, vm.Status);
            Assert.Equal(dto.Data, vm.Data);
        }

        [Fact]
        public void AutoMapper_MetaDtoParaViewModel_DeveMapearCorretamente()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            var mapper = config.CreateMapper();
            var dto = new MetaDto { Id = Guid.NewGuid(), Descricao = "Meta", ValorAlvo = 100, ValorAcumulado = 10, DataLimite = DateTime.Today.AddDays(10) };
            var vm = mapper.Map<MetaViewModel>(dto);
            Assert.Equal(dto.Id, vm.Id);
            Assert.Equal(dto.Descricao, vm.Descricao);
            Assert.Equal(dto.ValorAlvo, vm.ValorAlvo);
            Assert.Equal(dto.ValorAcumulado, vm.ValorAcumulado);
            Assert.Equal(dto.DataLimite, vm.DataLimite);
        }

        [Fact]
        public void AutoMapper_ParcelaDtoParaViewModel_DeveMapearCorretamente()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            var mapper = config.CreateMapper();
            var dto = new ParcelaDto { Id = Guid.NewGuid(), TransacaoId = Guid.NewGuid(), Numero = 1, Valor = 10, DataVencimento = DateTime.Today, Status = 1 };
            var vm = mapper.Map<ParcelaViewModel>(dto);
            Assert.Equal(dto.Id, vm.Id);
            Assert.Equal(dto.TransacaoId, vm.TransacaoId);
            Assert.Equal(dto.Numero, vm.Numero);
            Assert.Equal(dto.Valor, vm.Valor);
            Assert.Equal(dto.DataVencimento, vm.DataVencimento);
            Assert.Equal(dto.Status, vm.Status);
        }

        [Fact]
        public void AutoMapper_CategoriaDtoParaViewModel_DeveMapearCorretamente()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            var mapper = config.CreateMapper();
            var dto = new CategoriaDto { Id = Guid.NewGuid(), Nome = "Categoria", Tipo = 1 };
            var vm = mapper.Map<CategoriaViewModel>(dto);
            Assert.Equal(dto.Id, vm.Id);
            Assert.Equal(dto.Nome, vm.Nome);
            Assert.Equal(dto.Tipo, vm.Tipo);
        }
    }
}
