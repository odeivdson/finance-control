using Application.Models;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly FinanceDbContext _context;
        public CategoriaRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<CategoriaDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Categorias.FindAsync(id);
            return entity == null ? null : new CategoriaDto
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Tipo = (int)entity.Tipo
            };
        }

        public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            return await _context.Categorias.Select(entity => new CategoriaDto
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Tipo = (int)entity.Tipo
            }).ToListAsync();
        }

        public async Task AddAsync(CategoriaDto dto)
        {
            var entity = new Categoria
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Nome = dto.Nome,
                Tipo = (TipoTransacao)dto.Tipo
            };
            _context.Categorias.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoriaDto dto)
        {
            var entity = await _context.Categorias.FindAsync(dto.Id);
            if (entity != null)
            {
                entity.Nome = dto.Nome;
                entity.Tipo = (TipoTransacao)dto.Tipo;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Categorias.FindAsync(id);
            if (entity != null)
            {
                _context.Categorias.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
