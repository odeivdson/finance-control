using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class TransacaoViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Descri��o obrigat�ria")]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data obrigat�ria")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Valor obrigat�rio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Categoria obrigat�ria")]
        public Guid CategoriaId { get; set; }

        [Required]
        public int Tipo { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
