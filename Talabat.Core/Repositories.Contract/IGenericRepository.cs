using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        Task<T> GetAsync(int id);
        Task<T> GetWithSpecAsync(ISpecification<T> spec);
        Task<T> GetByNameAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        void Delete(int id);

        void Add(T entity);

        void Update(T entity);



    }
}
