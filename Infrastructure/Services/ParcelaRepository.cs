using Application.Models;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ParcelaRepository : IParcelaRepository
    {
        private readonly FinanceDbContext _context;
        public ParcelaRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<ParcelaDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Parcelas.FindAsync(id);
            return entity == null ? null : new ParcelaDto
            {
                Id = entity.Id,
                TransacaoId = entity.TransacaoId,
                Numero = entity.Numero,
                Valor = entity.Valor,
                DataVencimento = entity.DataVencimento,
                Status = (int)entity.Status
            };
        }

        public async Task<IEnumerable<ParcelaDto>> GetByTransacaoIdAsync(Guid transacaoId)
        {
            return await _context.Parcelas.Where(p => p.TransacaoId == transacaoId)
                .Select(entity => new ParcelaDto
                {
                    Id = entity.Id,
                    TransacaoId = entity.TransacaoId,
                    Numero = entity.Numero,
                    Valor = entity.Valor,
                    DataVencimento = entity.DataVencimento,
                    Status = (int)entity.Status
                }).ToListAsync();
        }

        public async Task<IEnumerable<ParcelaDto>> GetAllAsync()
        {
            return await _context.Parcelas.Select(entity => new ParcelaDto
            {
                Id = entity.Id,
                TransacaoId = entity.TransacaoId,
                Numero = entity.Numero,
                Valor = entity.Valor,
                DataVencimento = entity.DataVencimento,
                Status = (int)entity.Status
            }).ToListAsync();
        }

        public async Task AddAsync(ParcelaDto dto)
        {
            var entity = new Parcela
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                TransacaoId = dto.TransacaoId,
                Numero = dto.Numero,
                Valor = dto.Valor,
                DataVencimento = dto.DataVencimento,
                Status = (StatusTransacao)dto.Status
            };
            _context.Parcelas.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ParcelaDto dto)
        {
            var entity = await _context.Parcelas.FindAsync(dto.Id);
            if (entity != null)
            {
                entity.TransacaoId = dto.TransacaoId;
                entity.Numero = dto.Numero;
                entity.Valor = dto.Valor;
                entity.DataVencimento = dto.DataVencimento;
                entity.Status = (StatusTransacao)dto.Status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Parcelas.FindAsync(id);
            if (entity != null)
            {
                _context.Parcelas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}