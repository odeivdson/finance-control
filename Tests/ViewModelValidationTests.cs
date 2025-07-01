using Xunit;
using Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tests
{
    public class ViewModelValidationTests
    {
        [Fact]
        public void ParcelaViewModel_ValidModel_DeveSerValido()
        {
            var model = new ParcelaViewModel
            {
                Id = Guid.NewGuid(),
                TransacaoId = Guid.NewGuid(),
                Numero = 1,
                Valor = 10,
                DataVencimento = DateTime.Today.AddDays(1),
                Status = 1
            };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results, true);
            Assert.True(isValid);
        }

        [Fact]
        public void ParcelaViewModel_ValorZero_DeveSerInvalido()
        {
            var model = new ParcelaViewModel
            {
                Id = Guid.NewGuid(),
                TransacaoId = Guid.NewGuid(),
                Numero = 1,
                Valor = 0,
                DataVencimento = DateTime.Today.AddDays(1),
                Status = 1
            };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results, true);
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Valor deve ser maior que zero"));
        }

        [Fact]
        public void MetaViewModel_ValidModel_DeveSerValido()
        {
            var model = new MetaViewModel
            {
                Id = Guid.NewGuid(),
                Descricao = "Meta",
                ValorAlvo = 100,
                ValorAcumulado = 10,
                DataLimite = DateTime.Today.AddDays(10)
            };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results, true);
            Assert.True(isValid);
        }
    }
}
