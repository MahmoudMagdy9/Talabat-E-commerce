using AutoMapper;
using Talabat.API.DTO;
using Talabat.Core.Entities;

namespace Talabat.API.Helpers
{
    public class ProductPicUrlResolver : IValueResolver<Product ,ProductDTO,string>
    {
        private readonly IConfiguration _configuration;

        public ProductPicUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["APIUrl"]}/{source.PictureUrl}";

            return string.Empty;
        }
    }
}
