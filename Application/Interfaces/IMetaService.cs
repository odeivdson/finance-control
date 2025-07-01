using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMetaService
    {
        Task<IEnumerable<MetaDto>> GetAllAsync();
        Task<MetaDto?> GetByIdAsync(Guid id);
        Task AddAsync(MetaDto dto);
        Task UpdateAsync(MetaDto dto);
        Task DeleteAsync(Guid id);
    }
}
