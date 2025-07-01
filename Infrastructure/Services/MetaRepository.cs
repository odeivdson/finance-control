using Application.Models;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class MetaRepository : IMetaRepository
    {
        private readonly FinanceDbContext _context;
        public MetaRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<MetaDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Metas.FindAsync(id);
            return entity == null ? null : new MetaDto
            {
                Id = entity.Id,
                Descricao = entity.Descricao,
                ValorAlvo = entity.ValorAlvo,
                ValorAcumulado = entity.ValorAcumulado,
                DataLimite = entity.DataLimite
            };
        }

        public async Task<IEnumerable<MetaDto>> GetAllAsync()
        {
            return await _context.Metas.Select(entity => new MetaDto
            {
                Id = entity.Id,
                Descricao = entity.Descricao,
                ValorAlvo = entity.ValorAlvo,
                ValorAcumulado = entity.ValorAcumulado,
                DataLimite = entity.DataLimite
            }).ToListAsync();
        }

        public async Task AddAsync(MetaDto dto)
        {
            var entity = new Meta
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Descricao = dto.Descricao,
                ValorAlvo = dto.ValorAlvo,
                ValorAcumulado = dto.ValorAcumulado,
                DataLimite = dto.DataLimite
            };
            _context.Metas.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MetaDto dto)
        {
            var entity = await _context.Metas.FindAsync(dto.Id);
            if (entity != null)
            {
                entity.Descricao = dto.Descricao;
                entity.ValorAlvo = dto.ValorAlvo;
                entity.ValorAcumulado = dto.ValorAcumulado;
                entity.DataLimite = dto.DataLimite;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Metas.FindAsync(id);
            if (entity != null)
            {
                _context.Metas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
