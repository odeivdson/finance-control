using Application.Models;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly FinanceDbContext _context;
        public TransacaoRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<TransacaoDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Transacoes.FindAsync(id);
            return entity == null ? null : new TransacaoDto
            {
                Id = entity.Id,
                Descricao = entity.Descricao,
                Data = entity.Data,
                Valor = entity.Valor,
                CategoriaId = entity.CategoriaId,
                Tipo = (int)entity.Tipo,
                Status = (int)entity.Status
            };
        }

        public async Task<IEnumerable<TransacaoDto>> GetAllAsync()
        {
            return await _context.Transacoes.Select(entity => new TransacaoDto
            {
                Id = entity.Id,
                Descricao = entity.Descricao,
                Data = entity.Data,
                Valor = entity.Valor,
                CategoriaId = entity.CategoriaId,
                Tipo = (int)entity.Tipo,
                Status = (int)entity.Status
            }).ToListAsync();
        }

        public async Task AddAsync(TransacaoDto dto)
        {
            var entity = new Transacao
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Descricao = dto.Descricao,
                Data = dto.Data,
                Valor = dto.Valor,
                CategoriaId = dto.CategoriaId,
                Tipo = (TipoTransacao)dto.Tipo,
                Status = (StatusTransacao)dto.Status
            };
            _context.Transacoes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TransacaoDto dto)
        {
            var entity = await _context.Transacoes.FindAsync(dto.Id);
            if (entity != null)
            {
                entity.Descricao = dto.Descricao;
                entity.Data = dto.Data;
                entity.Valor = dto.Valor;
                entity.CategoriaId = dto.CategoriaId;
                entity.Tipo = (TipoTransacao)dto.Tipo;
                entity.Status = (StatusTransacao)dto.Status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Transacoes.FindAsync(id);
            if (entity != null)
            {
                _context.Transacoes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
