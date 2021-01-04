using System;
using System.Collections.Generic;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Shared.Dto.Orders
{
    public record OrderDto
    {
        public long Id { get; init; }
        public DateTime DateCreated { get; init; }
        public DateTime DateModified { get; init; }
        public bool IsRemoved { get; init; }

        public DateTime DeliveryDate { get; init; }

        //  public OrderStatus Status { get; set; }

        public long UserId { get; init; }
        public UserDto User { get; init; }

        public long DestinationCompanyId { get; init; }
        public CompanyDto DestinationCompany { get; init; }

        public IEnumerable<OrderProductDto> OrderProducts { get; set; }
    }
}