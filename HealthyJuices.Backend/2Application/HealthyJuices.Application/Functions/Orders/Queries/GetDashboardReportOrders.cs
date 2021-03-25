using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto;
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
            private readonly IMapper _mapper;

            public Handler(IOrderRepository repository, IMapper mapper)
            {
                this._orderRepository = repository;
                _mapper = mapper;
            }

            public async Task<DashboardOrderReportDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var orders = await _orderRepository.GetAllActiveWithRelationsAsync(request.From, request.To);

                var ordersBycompany = orders
                    .GroupBy(x => x.User.Company)
                    .Select(x => new OrderReportDto()
                    {
                        Company = _mapper.Map<CompanyDto>(x.Key),
                        ProductsByUser = x.GroupBy(z => z.User).Select(u => new UsersProductsReportDto()
                        {
                            User = new UserDto
                            {
                                Email = u.Key.Email
                            },
                            Products = u.SelectMany(p => p.OrderProducts.Select(z => new OrderItemDto()
                            {
                                Product = new ProductDto()
                                {
                                    Name = z.Product.Name
                                },
                                Amount = z.Amount
                            }))
                        })
                    }).ToList();

                var products = orders
                    .SelectMany(x => x.OrderProducts)
                    .GroupBy(z => z.ProductId)
                    .Select(x => new OrderItemDto()
                    {
                        Amount = x.Sum(a => a.Amount),
                        ProductId = x.Key,
                        Product = _mapper.Map<ProductDto>(x.First().Product)
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