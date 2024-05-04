using AutoMapper;
using Talabat.API.DTO;
using Talabat.Core.Entities;

namespace Talabat.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.Brand,
                    o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category,
                    o => o.MapFrom(s => s.Category.Name))
                .ForMember(d=>d.PictureUrl , o=>o.MapFrom(s=>$"{"http://localhost:5182"}/{s.PictureUrl}"));
                
        }
    }
}
