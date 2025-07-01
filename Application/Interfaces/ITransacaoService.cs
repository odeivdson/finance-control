using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransacaoService
    {
        Task<IEnumerable<TransacaoDto>> GetAllAsync(TransacaoFiltroDto? filtro = null);
        Task<TransacaoDto?> GetByIdAsync(Guid id);
        Task AddAsync(TransacaoDto dto);
        Task UpdateAsync(TransacaoDto dto);
        Task DeleteAsync(Guid id);
    }
}
