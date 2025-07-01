using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class ParcelaViewModel
    {
        public Guid Id { get; set; }
        public Guid TransacaoId { get; set; }
        [Required]
        public int Numero { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DataVencimento { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
