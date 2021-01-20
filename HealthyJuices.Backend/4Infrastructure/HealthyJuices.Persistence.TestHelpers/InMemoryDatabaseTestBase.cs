using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Logs.DataAccess;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Domain.Services;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Persistence.Ef.Repositories.Companies;
using HealthyJuices.Persistence.Ef.Repositories.Logs;
using HealthyJuices.Persistence.Ef.Repositories.Orders;
using HealthyJuices.Persistence.Ef.Repositories.Products;
using HealthyJuices.Persistence.Ef.Repositories.Unavailabilities;
using HealthyJuices.Persistence.Ef.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;

namespace HealthyJuices.Persistence.TestHelpers
{
    public abstract class InMemoryDatabaseTestBase : IDisposable
    {
        public IDbContext ArrangeRepositoryContext { get; set; }
        public IDbContext ActRepositoryContext { get; set; }
        public IDbContext AssertRepositoryContext { get; set; }


        public IUserRepository UserRepository { get; set; }
        public ICompanyRepository CompanyRepository { get; set; }
        public ILogRepository LogRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IUnavailabilityRepository UnavailabilityRepository { get; set; }


        public Mock<IMailer> MailerMock { get; set; }
        public Mock<ITimeProvider> TimeProviderMock { get; set; }


        protected InMemoryDatabaseTestBase()
        {
            InitializeDbContexts();
            InitializeRepositories();
            InitializeServices();
            SeedGlobalInitData();
        }

        private void SeedGlobalInitData()
        {
        }

        private void InitializeDbContexts()
        {
            var options = new DbContextOptionsBuilder<HealthyJuicesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            ArrangeRepositoryContext = new HealthyJuicesDbContext(options);
            ActRepositoryContext = new HealthyJuicesDbContext(options);
            AssertRepositoryContext = new HealthyJuicesDbContext(options);
        }

        private void InitializeRepositories()
        {
            UserRepository = new UserRepository(ActRepositoryContext, TimeProviderMock.Object);
            CompanyRepository = new CompanyRepository(ActRepositoryContext, TimeProviderMock.Object);
            LogRepository = new LogRepository(ActRepositoryContext, TimeProviderMock.Object);
            OrderRepository = new OrderRepository(ActRepositoryContext, TimeProviderMock.Object);
            ProductRepository = new ProductRepository(ActRepositoryContext, TimeProviderMock.Object);
            UnavailabilityRepository = new UnavailabilityRepository(ActRepositoryContext, TimeProviderMock.Object);
        }

        private void InitializeServices()
        {
            MailerMock = new Mock<IMailer>();
            TimeProviderMock = new Mock<ITimeProvider>();
        }


        public void Dispose()
        {
            ArrangeRepositoryContext.Dispose();
            AssertRepositoryContext.Dispose();
            ActRepositoryContext.Dispose();
        }
    }
}
