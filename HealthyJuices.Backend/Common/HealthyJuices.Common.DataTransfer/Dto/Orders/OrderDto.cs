using System;
using System.Collections.Generic;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Shared.Dto.Orders
{
    public record OrderDto
    {
        public string Id { get; init; }
        public DateTime Created { get; init; }
        public DateTime? LastModified { get; init; }
        public bool IsRemoved { get; init; }

        public DateTime DeliveryDate { get; init; }

        public string UserId { get; init; }
        public UserDto User { get; init; }

        public string DestinationCompanyId { get; init; }
        public CompanyDto DestinationCompany { get; init; }

        public IEnumerable<OrderItemDto> OrderProducts { get; set; }
    }
}