using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specification;
using Talabat.Core.Specification.ProductsSepcs;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T?> GetAsync(int id)
        {
            //if (typeof(T) == typeof(Product))
            //    return await _dbContext.Set<Product>().Where(p => p.Id == id).Include(b => b.Brand)
            //        .Include(c => c.Category).FirstOrDefaultAsync();
           

            return await _dbContext.Set<T>().FindAsync(id);

        }

        public async Task<T> GetWithSpecAsync(ISpecification<T> spec)
        {
             return await ApplySecifications(spec).FirstOrDefaultAsync();
        }

        public Task<T> GetByNameAsync(string id)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Product))
                return (IEnumerable<T>)await _dbContext.Set<Product>().Include(b=>b.Brand).Include(c=>c.Category).ToListAsync();

            return await _dbContext.Set<T>().ToListAsync();

        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {

            return await ApplySecifications(spec).ToListAsync();
        }

        private IQueryable<T> ApplySecifications(ISpecification<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
