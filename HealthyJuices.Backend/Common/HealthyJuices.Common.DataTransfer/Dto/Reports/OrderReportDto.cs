using System.Collections.Generic;

namespace HealthyJuices.Shared.Dto.Reports
{
    public record OrderReportDto
    {
        public CompanyDto Company { get; init; }
        public IEnumerable<UsersProductsReportDto> ProductsByUser { get; init; }
    }
}