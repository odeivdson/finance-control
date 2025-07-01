using System;

namespace Application.Models
{
    public class TransacaoDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public Guid CategoriaId { get; set; }
        public int Tipo { get; set; }
        public int Status { get; set; }
    }
}
