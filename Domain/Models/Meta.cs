using System;

namespace Domain.Models
{
    public class Meta
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal ValorAlvo { get; set; }
        public decimal ValorAcumulado { get; set; }
        public DateTime DataLimite { get; set; }
    }
}
