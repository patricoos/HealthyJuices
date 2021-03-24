using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Licences;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HealthyJuices.Persistence.MongoDb
{
    public class MongoDbClient : IMongoDbClient
    {
        public IMongoCollection<Log> Logs { get; init; }
        public  IMongoCollection<Order> Orders { get; init; }
        public  IMongoCollection<Unavailability> Unavailabilities { get; init; }
        public  IMongoCollection<Company> Companies { get; init; }
        public  IMongoCollection<User> Users { get; init; }
        public  IMongoCollection<Product> Products { get; init; }
        public  IMongoCollection<OrderItem> OrderItems { get; init; }


        public MongoDbClient(IOptions<MongoDbLicences> bookstoreDbConfig)
        {
            var client = new MongoClient(bookstoreDbConfig.Value.ConnectionString);
            var database = client.GetDatabase(bookstoreDbConfig.Value.DatabaseName);

            Logs = database.GetCollection<Log>(nameof(Logs));
            Orders = database.GetCollection<Order>(nameof(Orders));
            Unavailabilities = database.GetCollection<Unavailability>(nameof(Unavailabilities));
            Companies = database.GetCollection<Company>(nameof(Companies));
            Users = database.GetCollection<User>(nameof(Users));
            Products = database.GetCollection<Product>(nameof(Products));
            OrderItems = database.GetCollection<OrderItem>(nameof(OrderItems));
        }
    }
}