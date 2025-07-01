using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMetaRepository
    {
        Task<IEnumerable<MetaDto>> GetAllAsync();
        Task<MetaDto?> GetByIdAsync(Guid id);
        Task AddAsync(MetaDto dto);
        Task UpdateAsync(MetaDto dto);
        Task DeleteAsync(Guid id);
    }
}
