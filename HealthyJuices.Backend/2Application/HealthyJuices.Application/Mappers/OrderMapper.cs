﻿using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Shared.Dto;
using Nexus.Application.Mappers;

namespace HealthyJuices.Application.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this Order e) => new OrderDto()
        {
            Id = e.Id,
            DateCreated = e.DateCreated,
            DateModified = e.DateModified,
            IsRemoved = e.IsRemoved,

            DeliveryDate = e.DeliveryDate,
            UserId = e.UserId,
            User = e.User?.ToDto(),
            DestinationCompanyId = e.DestinationCompanyId,
            DestinationCompany = e.DestinationCompany?.ToDto()
        };
    }
}