using System.Collections.Generic;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Shared.Dto.Reports
{
    public record UsersProductsReportDto
    {
        public UserDto User { get; init; }
        public IEnumerable<OrderProductDto> Products { get; init; }
    }
}