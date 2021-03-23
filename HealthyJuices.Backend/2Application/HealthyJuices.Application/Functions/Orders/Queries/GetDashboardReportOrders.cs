using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto.Products;
using HealthyJuices.Shared.Dto.Reports;
using MediatR;

namespace HealthyJuices.Application.Functions.Orders.Queries
{
    public static class GetDashboardReportOrders
    {
        // Query 
        public record Query(DateTime From, DateTime To) : IRequest<DashboardOrderReportDto> { }

        // Handler
        public class Handler : IRequestHandler<Query, DashboardOrderReportDto>
        {
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository repository)
            {
                this._orderRepository = repository;
            }

            public async Task<DashboardOrderReportDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var orders = await _orderRepository.GetAllActiveWithRelationsAsync(request.From, request.To);

                var ordersBycompany = orders
                    .GroupBy(x => x.User.Company)
                    .Select(x => new OrderReportDto()
                    {
                        Company = x.Key.ToDto(),
                        ProductsByUser = x.GroupBy(z => z.User).Select(u => new UsersProductsReportDto()
                        {
                            User = u.Key.ToDto(),
                            Products = u.SelectMany(p => p.OrderProducts.Select(z => z.ToDto()))
                        })
                    }).ToList();

                var products = orders
                    .SelectMany(x => x.OrderProducts)
                    .GroupBy(z => z.ProductId)
                    .Select(x => new OrderItemDto()
                    {
                        Amount = x.Sum(a => a.Amount),
                        ProductId = x.Key,
                        Product = x.First().Product.ToDto()
                    }).ToList();

                var result = new DashboardOrderReportDto()
                {
                    OrderReports = ordersBycompany,
                    Products = products
                };

                return result;
            }
        }
    }
}