using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Controllers
{
    public class OrdersController
    {
        private readonly IOrderRepository _orderRepository;

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

        public async Task<List<OrderDto>> GetAllActiveAsync()
        {
            var entities = await _orderRepository.Query()
                .IsNotRemoved()
                .ToListAsync();

            var result = entities
                .Select(x => x.ToDto())
                .ToList();

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