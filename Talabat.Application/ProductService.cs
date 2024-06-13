using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specification.ProductsSpecs;

namespace Talabat.Application
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParameters specParams)
        {
            var spec = new ProductWithBrandAndCategorySpec(specParams);

            return await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            var spec = new ProductWithBrandAndCategorySpec(id);
            return await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);

        }

        public Task<int> GetCountAsync(ProductSpecParameters specParams)
        {
            var countSpec = new ProductWithFiltrationForCountSpec(specParams);
            return _unitOfWork.Repository<Product>().GetCountAsync(countSpec);

        }

         public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
            => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();


        public Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
            => _unitOfWork.Repository<ProductCategory>().GetAllAsync();
    }
}
