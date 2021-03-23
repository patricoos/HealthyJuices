using System;
using System.Collections.Generic;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Shared.Dto.Orders
{
    public class CreateOrderDto
    {
        public DateTime DeliveryDate { get; init; }
        public IEnumerable<OrderItemDto> OrderProducts { get; set; }
    }
}