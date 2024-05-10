using System.Diagnostics;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification.ProductsSpecs
{
    public class ProductWithBrandAndCategorySpec : BaseSpecification<Product>
    {
        public ProductWithBrandAndCategorySpec(ProductSpecParameters specParams) : base(p =>
            (string.IsNullOrEmpty(specParams.Search) || p.Name_Normalized.Contains(specParams.Search) &&
            (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId) &&
            (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId)))
        {
            AddIncludes();
            if (!string.IsNullOrEmpty(specParams.Sort))
                switch (specParams.Sort)
                {
                    case "price":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceAsc":
                        AddOrderByDes(p => p.Price);
                        break;
                    default:
                        AddOrderByDes(p => p.Name);
                        break;
                }

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize , specParams.PageSize);
        }
        public ProductWithBrandAndCategorySpec(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }
    }
}
