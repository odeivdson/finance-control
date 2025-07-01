using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class TransacaoViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Descrição obrigatória")]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data obrigatória")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Valor obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Categoria obrigatória")]
        public Guid CategoriaId { get; set; }

        [Required]
        public int Tipo { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
