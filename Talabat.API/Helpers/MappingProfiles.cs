using AutoMapper;
using Talabat.API.DTO;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Address = Talabat.Core.Entities.Identity.Address;

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
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => $"{"http://localhost:5182"}/{s.PictureUrl}"));

            CreateMap<CustomerBasetDTO, CustomerBasket>();
            CreateMap<BasketItemDTO, BasketItem>();
            CreateMap<Address, AddressDTO>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));
       
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d=>d.ProductName , o=>o.MapFrom(s=>s.Product.ProductName))
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d=>d.PictureUrl , o=>o.MapFrom<OrderItemPictureUrlResolver>());

        }

    }
}
