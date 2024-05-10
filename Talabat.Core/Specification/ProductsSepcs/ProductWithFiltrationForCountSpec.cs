using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification.ProductsSpecs
{
    public class ProductWithFiltrationForCountSpec(ProductSpecParameters specParams) : BaseSpecification<Product>(p =>

        (string.IsNullOrEmpty(specParams.Search) || p.Name_Normalized.Contains(specParams.Search) &&
        (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId) &&
        (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId)));
}
