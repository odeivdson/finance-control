using System;

namespace Domain.Models
{
    public class Parcela
    {
        public Guid Id { get; set; }
        public Guid TransacaoId { get; set; }
        public Transacao Transacao { get; set; } = null!;
        public int Numero { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public StatusTransacao Status { get; set; }
    }
}
