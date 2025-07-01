using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<IEnumerable<TransacaoDto>> GetAllAsync();
        Task<TransacaoDto?> GetByIdAsync(Guid id);
        Task AddAsync(TransacaoDto dto);
        Task UpdateAsync(TransacaoDto dto);
        Task DeleteAsync(Guid id);
    }
}
