using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class CategoriaViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;
        public int Tipo { get; set; }
    }
}
