using System.Collections.Generic;
using HealthyJuices.Shared.Dto.Orders;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Shared.Dto.Reports
{
    public record DashboardOrderReportDto
    {
        public IEnumerable<OrderReportDto> OrderReports { get; init; }
        public IEnumerable<OrderItemDto> Products { get; init; }
    }
}