using API.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryToReturnDto>()
                .ForMember(d => d.ParentName, o => o.MapFrom(s => s.Parent.Name));
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));
            //I use VehicleImageUrlResolver because we don't need to read the Config file all the time and this help us to reduce using the RAM
            CreateMap<Vehicle, VehicleToReturnDto>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<VehicleImageUrlResolver>());
            CreateMap<Warehouse, WarehouseToReturnDto>();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO, Address>().ReverseMap();
            CreateMap<OrderAddress, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO, OrderAddress>();
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderUrlResolver>());
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price));
        }
    }
}
