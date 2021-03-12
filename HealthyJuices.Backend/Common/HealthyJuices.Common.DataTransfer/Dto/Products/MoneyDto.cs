using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Shared.Dto.Products
{
    public class MoneyDto
    {
        public decimal Amount { get;  init; }
        public Currency Currency { get; init; }
    }
}