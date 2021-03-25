using System;
using System.Linq.Expressions;
using AutoMapper;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Dto.Orders;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.Created, y => y.MapFrom(p => p.Created.UtcDateTime))
                .ForMember(x => x.LastModified, y => y.MapFrom(p => p.LastModified.HasValue ? p.LastModified.Value.UtcDateTime : (DateTime?)null));

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.Created, y => y.MapFrom(p => p.Created.UtcDateTime))
                .ForMember(x => x.LastModified, y => y.MapFrom(p => p.LastModified.HasValue ? p.LastModified.Value.UtcDateTime : (DateTime?)null)); ;
           
            CreateMap<Unavailability, UnavailabilityDto>();

            CreateMap<Order, OrderDto>()
                .ForMember(x => x.Created, y => y.MapFrom(p => p.Created.UtcDateTime))
                .ForMember(x => x.LastModified, y => y.MapFrom(p => p.LastModified.HasValue ? p.LastModified.Value.UtcDateTime : (DateTime?)null));

            CreateMap<Money, MoneyDto>();

            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<Company, CompanyDto>()
                .ForMember(x => x.Created, y => y.MapFrom(p => p.Created.UtcDateTime))
                .ForMember(x => x.LastModified, y => y.MapFrom(p => p.LastModified.HasValue ? p.LastModified.Value.UtcDateTime : (DateTime?)null));
        }
    }
}