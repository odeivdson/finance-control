using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Transacao
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
        public TipoTransacao Tipo { get; set; }
        public StatusTransacao Status { get; set; }
        public ICollection<Parcela> Parcelas { get; set; } = new List<Parcela>();
    }
}
