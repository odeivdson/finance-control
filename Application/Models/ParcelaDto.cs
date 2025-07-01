using System;

namespace Application.Models
{
    public class ParcelaDto
    {
        public Guid Id { get; set; }
        public Guid TransacaoId { get; set; }
        public int Numero { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public int Status { get; set; }
    }
}
