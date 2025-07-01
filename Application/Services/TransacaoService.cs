using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;

        public TransacaoService(ITransacaoRepository transacaoRepository)
        {
            _transacaoRepository = transacaoRepository;
        }

        public async Task<IEnumerable<TransacaoDto>> GetAllAsync(TransacaoFiltroDto? filtro = null)
        {
            var transacoes = await _transacaoRepository.GetAllAsync();
            var query = transacoes.AsQueryable();

            if (filtro != null)
            {
                if (filtro.DataInicio.HasValue)
                    query = query.Where(t => t.Data >= filtro.DataInicio.Value);
                if (filtro.DataFim.HasValue)
                    query = query.Where(t => t.Data <= filtro.DataFim.Value);
                if (filtro.Tipo.HasValue)
                    query = query.Where(t => t.Tipo == filtro.Tipo.Value);
                if (filtro.Status.HasValue)
                    query = query.Where(t => t.Status == filtro.Status.Value);
                if (filtro.CategoriaId.HasValue)
                    query = query.Where(t => t.CategoriaId == filtro.CategoriaId.Value);
            }

            return query.ToList();
        }

        public Task<TransacaoDto?> GetByIdAsync(Guid id)
        {
            return _transacaoRepository.GetByIdAsync(id);
        }

        public Task AddAsync(TransacaoDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Descricao)) throw new ArgumentException("Descri��o � obrigat�ria");
            if (dto.Valor <= 0) throw new ArgumentException("Valor deve ser maior que zero");
            if (dto.CategoriaId == Guid.Empty) throw new ArgumentException("CategoriaId inv�lido");
            if (dto.Tipo != 1 && dto.Tipo != 2) throw new ArgumentException("Tipo inv�lido");
            return _transacaoRepository.AddAsync(dto);
        }

        public Task UpdateAsync(TransacaoDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Descricao)) throw new ArgumentException("Descri��o � obrigat�ria");
            if (dto.Valor <= 0) throw new ArgumentException("Valor deve ser maior que zero");
            if (dto.CategoriaId == Guid.Empty) throw new ArgumentException("CategoriaId inv�lido");
            if (dto.Tipo != 1 && dto.Tipo != 2) throw new ArgumentException("Tipo inv�lido");
            return _transacaoRepository.UpdateAsync(dto);
        }

        public Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id inv�lido");
            return _transacaoRepository.DeleteAsync(id);
        }
    }
}
