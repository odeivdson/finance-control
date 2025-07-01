using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<CategoriaDto>> GetAllAsync();
        Task<CategoriaDto?> GetByIdAsync(Guid id);
        Task AddAsync(CategoriaDto dto);
        Task UpdateAsync(CategoriaDto dto);
        Task DeleteAsync(Guid id);
    }
}
