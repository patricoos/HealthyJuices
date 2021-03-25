using System.Linq;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Shared.Dto.Orders;

namespace HealthyJuices.Application.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this Order e) => new OrderDto()
        {
            Id = e.Id,
            Created = e.Created.UtcDateTime,
            LastModified = e.LastModified?.UtcDateTime,
            IsRemoved = e.IsRemoved,

            DeliveryDate = e.DeliveryDate,
            UserId = e.UserId,
            User = e.User?.ToDto(),
            DestinationCompanyId = e.DestinationCompanyId,
            DestinationCompany = e.DestinationCompany?.ToDto(),
            OrderProducts = e.OrderProducts?.Select(x => x.ToDto()).ToList()
        };
    }
}