using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Categoria
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public TipoTransacao Tipo { get; set; }
        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    }
}
