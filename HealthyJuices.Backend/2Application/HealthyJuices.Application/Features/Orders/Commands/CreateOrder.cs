using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto.Orders;
using MediatR;

namespace HealthyJuices.Application.Features.Orders.Commands
{
    public static class CreateOrder
    {
        // Command 
        public record Command : OrderDto, IRequest<string> { }

        // Handler
        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IUserRepository _userRepository;
            private readonly IProductRepository _productRepository;
            private readonly IUnavailabilityRepository _unavailabilityRepository;

            public Handler(IOrderRepository repository, IUserRepository userRepository, IUnavailabilityRepository unavailabilityRepository, IProductRepository productRepository)
            {
                this._orderRepository = repository;
                _userRepository = userRepository;
                _unavailabilityRepository = unavailabilityRepository;
                _productRepository = productRepository;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdWithRelationsAsync(request.UserId);
                if (user == null || user.IsRemoved || !user.IsActive)
                    throw new BadRequestException("User not found");

                var unavailability = await _unavailabilityRepository.GetAllAsync(request.DeliveryDate, request.DeliveryDate);
                if (unavailability.Any())
                    throw new BadRequestException("Can not create order in unavailability duration");

                var productsEntities = await _productRepository.GetAllActiveByIdsAsync(request.OrderProducts.Select(p => p.ProductId).ToArray());

                var products = request.OrderProducts.GroupBy(x => x.ProductId).Select(x =>
                {
                    var prod = productsEntities.FirstOrDefault(p => p.Id == x.Key);
                    if (prod == null)
                        throw new BadRequestException("Product not found");

                    return new KeyValuePair<Product, decimal>(prod, x.Sum(a => a.Amount));
                }).ToArray();

                var order = new Order(user, request.DeliveryDate, products);

                _orderRepository.Insert(order);
                await _orderRepository.SaveChangesAsync();

                return order.Id;
            }
        }
    }
}