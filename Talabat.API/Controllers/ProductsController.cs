using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTO;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specification.ProductsSpecs;

namespace Talabat.API.Controllers
{
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)] // improve swagger documentations
    public class ProductsController : BaseController
    {


        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(
            IProductService productService,
            IMapper mapper)
        {

            _productService = productService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductByIdWithSpecs(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product, ProductDTO>(product));
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize]
        [HttpGet] // GET : /api/products
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] ProductSpecParameters specParams)
        {
            var products = await _productService.GetProductsAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(products);
            var count = await _productService.GetCountAsync(specParams);
            return Ok(new Pagination<ProductDTO>(specParams.PageSize, specParams.PageIndex, count, data));
        }

        [HttpGet($"brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var productBrands = await _productService.GetBrandsAsync();
            return Ok(productBrands);
        }

        [HttpGet($"categories")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllCategories()
        {
            var productBrands = await _productService.GetCategoriesAsync();
            return Ok(productBrands);
        }
    }
}
