using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MetaService : IMetaService
    {
        private readonly IMetaRepository _metaRepository;
        public MetaService(IMetaRepository metaRepository)
        {
            _metaRepository = metaRepository;
        }
        public Task<IEnumerable<MetaDto>> GetAllAsync()
        {
            return _metaRepository.GetAllAsync();
        }
        public Task<MetaDto?> GetByIdAsync(Guid id)
        {
            return _metaRepository.GetByIdAsync(id);
        }
        public Task AddAsync(MetaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Descricao)) throw new ArgumentException("Descrição é obrigatória");
            if (dto.ValorAlvo <= 0) throw new ArgumentException("Valor alvo deve ser maior que zero");
            if (dto.DataLimite == default) throw new ArgumentException("Data limite inválida");
            return _metaRepository.AddAsync(dto);
        }
        public Task UpdateAsync(MetaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Descricao)) throw new ArgumentException("Descrição é obrigatória");
            if (dto.ValorAlvo <= 0) throw new ArgumentException("Valor alvo deve ser maior que zero");
            if (dto.DataLimite == default) throw new ArgumentException("Data limite inválida");
            return _metaRepository.UpdateAsync(dto);
        }
        public Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id inválido");
            return _metaRepository.DeleteAsync(id);
        }
    }
}
