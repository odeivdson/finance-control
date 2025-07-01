using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IParcelaService
    {
        Task<IEnumerable<ParcelaDto>> GetAllAsync();
        Task<ParcelaDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ParcelaDto>> GetByTransacaoIdAsync(Guid transacaoId);
        Task AddAsync(ParcelaDto dto);
        Task UpdateAsync(ParcelaDto dto);
        Task DeleteAsync(Guid id);
    }
}
