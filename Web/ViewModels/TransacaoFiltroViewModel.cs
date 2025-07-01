using System;

namespace Web.ViewModels
{
    public class TransacaoFiltroViewModel
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int? Tipo { get; set; }
        public int? Status { get; set; }
        public Guid? CategoriaId { get; set; }
    }
}
