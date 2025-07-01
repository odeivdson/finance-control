using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            return _categoriaRepository.GetAllAsync();
        }
        public Task<CategoriaDto?> GetByIdAsync(Guid id)
        {
            return _categoriaRepository.GetByIdAsync(id);
        }
        public Task AddAsync(CategoriaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Nome)) throw new ArgumentException("Nome é obrigatório");
            if (dto.Tipo != 1 && dto.Tipo != 2) throw new ArgumentException("Tipo inválido");
            return _categoriaRepository.AddAsync(dto);
        }
        public Task UpdateAsync(CategoriaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Nome)) throw new ArgumentException("Nome é obrigatório");
            if (dto.Tipo != 1 && dto.Tipo != 2) throw new ArgumentException("Tipo inválido");
            return _categoriaRepository.UpdateAsync(dto);
        }
        public Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id inválido");
            return _categoriaRepository.DeleteAsync(id);
        }
    }
}
