using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto.Orders;
using HealthyJuices.Shared.Dto.Products;
using HealthyJuices.Shared.Dto.Reports;

namespace HealthyJuices.Application.Controllers
{
    public class OrdersController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var entities = await _orderRepository.Query()
                .ToListAsync();

            var result = entities
                .Select(x => x.ToDto())
                .ToList();

            return result;
        }

        public async Task<List<OrderDto>> GetAllActiveAsync(DateTime? from, DateTime? to)
        {
            var query = _orderRepository.Query()
                .IncludeUser()
                .IncludeDestinationCompany()
                .IsNotRemoved();

            if (from.HasValue)
                query.AfterDateTime(from.Value);

            if (to.HasValue)
                query.BeforeDateTime(to.Value);

            var entities = await query.ToListAsync();

            var result = entities
                .Select(x => x.ToDto())
                .ToList();

            return result;
        }

        public async Task<DashboardOrderReportDto> GetAllActiveWithProductsAsync(DateTime from, DateTime to)
        {
            var entities = await _orderRepository.Query()
                .IncludeUser()
                .IncludeDestinationCompany()
                .IncludeProducts()
                .IsNotRemoved()
                .BetweenDateTimes(from, to)
                .ToListAsync();

            var ordersBycompany = entities
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

            var products = entities
                .SelectMany(x => x.OrderProducts.GroupBy(z => z.Product))
                .Select(x => new OrderProductDto()
                     {
                    Amount = x.Sum(a => a.Amount),
                    ProductId = x.Key.Id,
                    Product = x.Key.ToDto()
                });

            var result = new DashboardOrderReportDto()
            {
                OrderReports = ordersBycompany,
                Products = products
            };

            return result;
        }

        public async Task<OrderDto> GetByIdAsync(long id)
        {
            var entity = await _orderRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            var result = entity?.ToDto();

            return result;
        }

        public async Task<long> CreateAsync(OrderDto dto)
        {
            var order = new Order();

            _orderRepository.Insert(order);
            await _orderRepository.SaveChangesAsync();

            return order.Id;
        }

        public async Task UpdateAsync(OrderDto dto)
        {
            var order = await _orderRepository.Query()
                .ById(dto.Id)
                .FirstOrDefaultAsync();

            if (order == null)
                throw new BadRequestException($"Not found order with id: {dto.Id}");

            order.Update(dto.DeliveryDate);

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(long id)
        {
            var order = await _orderRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            if (order == null)
                throw new BadRequestException($"Not found order with id: {id}");

            order.Remove();

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }
    }
}