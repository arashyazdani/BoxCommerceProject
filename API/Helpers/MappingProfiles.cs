using API.DTOs;
using AutoMapper;
using Domain.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryToReturnDTO>()
                .ForMember(d => d.ParentName, o => o.MapFrom(s => s.Parent.Name));
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));
            //I use VehicleImageUrlResolver because we don't need to read the Config file all the time and this help us to reduce using the RAM
            CreateMap<Vehicle, VehicleToReturnDTO>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<VehicleImageUrlResolver>());
            CreateMap<Warehouse, WarehouseToReturnDTO>();
        }
    }
}
