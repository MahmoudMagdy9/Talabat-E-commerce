using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTO;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specification.ProductsSpecs;

namespace Talabat.API.Controllers
{
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)] // improve swagger documentations
    public class ProductsController : BaseController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _brandsRepository;
        private readonly IGenericRepository<ProductCategory> _categoriesRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository,
            IGenericRepository<ProductBrand> brandsRepository,
            IGenericRepository<ProductCategory> categoriesRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _brandsRepository = brandsRepository;
            _categoriesRepository = categoriesRepository;
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
        public async Task<ActionResult<Pagination<ProductDTO>>> GetAllProductsWithSpecs([FromQuery] ProductSpecParameters specParams)
        {
            var spec = new ProductWithBrandAndCategorySpec(specParams);
            var products = await _productRepository.GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(products);
            var countSpec = new ProductWithFiltrationForCountSpec(specParams);
            var count = await _productRepository.GetCountAsync(countSpec);
            return Ok(new Pagination<ProductDTO>(specParams.PageSize, specParams.PageIndex, count , data));
        }

        [HttpGet($"brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var productBrands = await _brandsRepository.GetAllAsync();
            return Ok(productBrands);
        }

        [HttpGet($"categories")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllCategories()
        {
            var productBrands = await _categoriesRepository.GetAllAsync();
            return Ok(productBrands);
        }
    }
}
