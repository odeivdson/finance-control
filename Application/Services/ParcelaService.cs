using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ParcelaService : IParcelaService
    {
        private readonly IParcelaRepository _parcelaRepository;
        public ParcelaService(IParcelaRepository parcelaRepository)
        {
            _parcelaRepository = parcelaRepository;
        }
        public Task<IEnumerable<ParcelaDto>> GetAllAsync()
        {
            return _parcelaRepository.GetAllAsync();
        }
        public Task<ParcelaDto?> GetByIdAsync(Guid id)
        {
            return _parcelaRepository.GetByIdAsync(id);
        }
        public Task<IEnumerable<ParcelaDto>> GetByTransacaoIdAsync(Guid transacaoId)
        {
            return _parcelaRepository.GetByTransacaoIdAsync(transacaoId);
        }
        public Task AddAsync(ParcelaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.Valor <= 0) throw new ArgumentException("Valor deve ser maior que zero");
            if (dto.Numero <= 0) throw new ArgumentException("Número da parcela deve ser maior que zero");
            if (dto.DataVencimento == default) throw new ArgumentException("Data de vencimento inválida");
            return _parcelaRepository.AddAsync(dto);
        }
        public Task UpdateAsync(ParcelaDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.Valor <= 0) throw new ArgumentException("Valor deve ser maior que zero");
            if (dto.Numero <= 0) throw new ArgumentException("Número da parcela deve ser maior que zero");
            if (dto.DataVencimento == default) throw new ArgumentException("Data de vencimento inválida");
            return _parcelaRepository.UpdateAsync(dto);
        }
        public Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id inválido");
            return _parcelaRepository.DeleteAsync(id);
        }
    }
}
