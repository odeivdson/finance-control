using System;

namespace Application.Models
{
    public class CategoriaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Tipo { get; set; }
    }
}
