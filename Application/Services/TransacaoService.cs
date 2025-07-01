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
            // Regras de negócio podem ser aplicadas aqui
            return _transacaoRepository.AddAsync(dto);
        }

        public Task UpdateAsync(TransacaoDto dto)
        {
            // Regras de negócio podem ser aplicadas aqui
            return _transacaoRepository.UpdateAsync(dto);
        }

        public Task DeleteAsync(Guid id)
        {
            // Regras de negócio podem ser aplicadas aqui
            return _transacaoRepository.DeleteAsync(id);
        }
    }
}
