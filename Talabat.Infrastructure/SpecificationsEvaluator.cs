using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Infrastructure
{
    public class SpecificationsEvaluator<T> : BaseSpecification<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;
            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            query = spec.Includes.Aggregate(query,
                (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;
        }
    }

}
