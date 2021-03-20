using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto.Orders;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Application.Services.Orders.Commands
{
    public static class CreateOrder
    {
        // Command 
        public record Command : OrderDto, IRequestWrapper<string> { }

        // Handler
        public class Handler : IHandlerWrapper<Command, string>
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

            public async Task<Response<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var errors = new List<string>();
                var user = await _userRepository.Query().IsActive().ById(request.UserId).IncludeCompany().FirstOrDefaultAsync();
                if (user == null)
                    errors.Add("User not found");

                var unavailability = await _unavailabilityRepository.Query().BetweenDateTimes(request.DeliveryDate, request.DeliveryDate).FirstOrDefaultAsync();
                if (unavailability != null)
                    errors.Add($"Can not create order in unavailability duration: {unavailability.From} - {unavailability.To}");

                var productsEntities = await _productRepository.Query()
                    .IsNotRemoved()
                    .IsActive()
                    .ByIds(request.OrderProducts.Select(p => p.ProductId).ToArray())
                    .ToListAsync();

                var products = request.OrderProducts.GroupBy(x => x.ProductId).Select(x =>
                {
                    var prod = productsEntities.FirstOrDefault(p => p.Id == x.Key);
                    if (prod == null)
                        errors.Add($"Product {x.Key} not found");

                    return new KeyValuePair<Product, decimal>(prod, x.Sum(a => a.Amount));
                }).ToArray();

                if (errors.Any())
                    return Response<string>.Fail(ResponseStatus.ValidationError, errors.ToArray()); 

                var order = new Order(user, request.DeliveryDate, products);

                _orderRepository.Insert(order);
                await _orderRepository.SaveChangesAsync();

                return Response<string>.Success(order.Id);
            }
        }
    }
}