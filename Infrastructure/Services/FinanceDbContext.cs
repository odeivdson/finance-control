using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options) { }

        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Parcela> Parcelas { get; set; }
        public DbSet<Meta> Metas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurações adicionais podem ser feitas aqui
        }
    }
}
