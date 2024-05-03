using Talabat.Core.Entities;

namespace Talabat.Core.Specification.ProductsSepcs
{
    public class ProductWithBrandAndCategorySpec : BaseSpecification<Product>
    {
        public ProductWithBrandAndCategorySpec() : base()
        {
            AddIncludes();

        }
        public ProductWithBrandAndCategorySpec(int id) : base(P=>P.Id ==id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }
}
