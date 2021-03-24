using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Users;
using MongoDB.Driver;

namespace HealthyJuices.Persistence.MongoDb
{
    public interface IMongoDbClient
    {
        public IMongoCollection<Log> Logs { get; }
        public IMongoCollection<Order> Orders { get; }
        public IMongoCollection<Unavailability> Unavailabilities { get; }
        public IMongoCollection<Company> Companies { get; }
        public IMongoCollection<User> Users { get; }
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<OrderItem> OrderItems { get; }
    }
}