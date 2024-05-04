using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTO;
using Talabat.API.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specification.ProductsSepcs;

namespace Talabat.API.Controllers
{
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)] // improve swagger documentations
    public class ProductsController : BaseController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository,IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductByIdWithSpecs(int id)
        {
            var spec = new ProductWithBrandAndCategorySpec(id);
            var product = await _productRepository.GetByIdWithSpecAsync(spec);
            if (product == null)
                return NotFound(new ApiResponse(404));
            
            return Ok(_mapper.Map<Product, ProductDTO>(product));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProductsWithSpecs()
        {
            var spec = new ProductWithBrandAndCategorySpec();
            var products = await _productRepository.GetAllWithSpecAsync(spec);
            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products));
        }

    }
}
