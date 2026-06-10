using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = [];

        public UnitOfWork(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // check type of TEntity
            var typeName = typeof(TEntity).Name;
            if (_repositories.TryGetValue(typeName,out object? value))
                return (IGenericRepository<TEntity>)value;
            else
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(typeName, repository);
                return repository;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _dbContext.SaveChangesAsync(ct);

    }
}
