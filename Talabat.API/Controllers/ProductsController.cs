using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specification.ProductsSepcs;
using Talabat.Infrastructure;

namespace Talabat.API.Controllers
{
 
    public class ProductsController : BaseController
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductsController(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductWithSpecs(int id)
        {
            var spec = new ProductWithBrandAndCategorySpec(id);
            var product = await _productRepository.GetWithSpecAsync(spec);
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsWithSpecs()
        {
            var spec = new ProductWithBrandAndCategorySpec();
            var products = await _productRepository.GetAllWithSpecAsync(spec);
            return Ok(products);
        }

    }
}
