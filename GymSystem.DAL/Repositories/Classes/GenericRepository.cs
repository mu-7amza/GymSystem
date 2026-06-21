using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(GymDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
      
        public void AddAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.AddAsync(entity, ct);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbSet.AnyAsync(predicate, ct);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
        {
            return predicate == null ?
                _dbSet.AsNoTracking().CountAsync(ct) :
                _dbSet.AsNoTracking().CountAsync(predicate,ct);
        }

        public void DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Remove(entity);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken ct = default)
        {
            var query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(id, ct);
        }

        public void UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);
        }
    }
}
