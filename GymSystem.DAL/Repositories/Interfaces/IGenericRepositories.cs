using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        Task<TEntity> GetByIdAsync(int id, CancellationToken ct = default);
        void AddAsync(TEntity entity, CancellationToken ct = default);
        void UpdateAsync(TEntity entity, CancellationToken ct = default);
        void DeleteAsync(TEntity entity, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken ct = default);
        Task<int> CountAsync(Expression<Func<TEntity,bool>>? predicate = null,CancellationToken ct = default);
    }
}
