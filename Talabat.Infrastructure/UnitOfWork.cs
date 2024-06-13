using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure.Repository;

namespace Talabat.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
    
        private Hashtable _repositories; 

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(key))
                _repositories.Add(key, new GenericRepository<BaseEntity>(_dbContext));

            return (IGenericRepository<TEntity>)_repositories[key];
        }

        public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();
        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();


    }
}
