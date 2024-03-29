﻿using API.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.OrderAggregate;
using Domain.Specifications.CategorySpecifications;
using Domain.Specifications.ProductSpecifications;
using Domain.Specifications.VehicleSpecifications;
using Domain.Specifications.WarehouseSpecifications;
using Microsoft.AspNetCore.JsonPatch;

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
                .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver>());

            CreateMap<Vehicle, VehicleWithVehiclePartsToReturnDto>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver>())
                .ForMember(d=>d.VehicleParts,o=>o.MapFrom(s=>s.VehiclesParts));

            CreateMap<VehiclesPart, VehiclePartsDTO>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.Id))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                .ForMember(d => d.ProductPrice, o => o.MapFrom(s => s.Product.Price))
                .ForMember(d => d.ProductIsDiscontinued, o => o.MapFrom(s => s.Product.IsDiscontinued));

            CreateMap<Warehouse, WarehouseToReturnDto>();

            CreateMap<Vehicle, VehicleToReturnDto>()
                .ForMember(d=>d.PictureUrl, o => o.MapFrom<PictureUrlResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<Address, AddressDTO>().ReverseMap();

            CreateMap<AddressDTO, Address>().ReverseMap();

            CreateMap<OrderAddress, AddressDTO>().ReverseMap();

            CreateMap<AddressDTO, OrderAddress>();

            CreateMap<CreateCategoryParams, Category>();

            CreateMap<CreateProductParams, Product>();

            CreateMap<CreateWarehouseParams, Warehouse>();

            CreateMap<CreateVehicleParams, Vehicle>();

            CreateMap<UpdateCategoryParams, Category>();

            CreateMap<UpdateProductParams, Product>();

            CreateMap<UpdateWarehouseParams, Warehouse>();

            CreateMap<UpdateVehicleParams, Vehicle>();

            CreateMap<Category,UpdateCategoryParams>();

            CreateMap<Product, UpdateProductParams>();

            CreateMap<Warehouse, UpdateWarehouseParams>();

            CreateMap<Vehicle, UpdateVehicleParams>();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver>());

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price));

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.CategoryId, o=>o.MapFrom(s=>s.Category.Id))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));
            CreateMap<ProductToReturnDto, Product>()
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId));

        }
    }
}
