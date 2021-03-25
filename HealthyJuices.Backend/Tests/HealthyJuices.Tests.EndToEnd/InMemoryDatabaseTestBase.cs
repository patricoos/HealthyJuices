using System;
using HealthyJuices.Application.Auth;
using HealthyJuices.Application.Providers;

using HealthyJuices.Application.Utils;
using HealthyJuices.Common;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Providers;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Logs.DataAccess;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Domain.Providers;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Persistence.Ef.Repositories.Companies;
using HealthyJuices.Persistence.Ef.Repositories.Orders;
using HealthyJuices.Persistence.Ef.Repositories.Products;
using HealthyJuices.Persistence.Ef.Repositories.Unavailabilities;
using HealthyJuices.Persistence.Ef.Repositories.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HealthyJuices.Tests.EndToEnd
{
    public abstract class InMemoryDatabaseTestBase : IDisposable
    {
        #region Contexts
        public IApplicationDbContext ArrangeRepositoryContext { get; set; }
        public IApplicationDbContext ActRepositoryContext { get; set; }
        public IApplicationDbContext AssertRepositoryContext { get; set; }
        #endregion Contexts

        #region Repositories
        public IUserRepository UserRepository { get; set; }
        public ICompanyRepository CompanyRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IUnavailabilityRepository UnavailabilityRepository { get; set; }
        public Mock<ILogRepository> LogRepositoryMock { get; set; }
        #endregion Repositories

        #region Services
        public ServiceCollection ServiceCollection { get; set; }
        public IMediator Mediator { get; set; }
        #endregion Services

        #region Providers
        public Mock<IMailer> MailerMock { get; set; }
        public EmailProvider EmailProvider { get; set; }
        public Mock<IDateTimeProvider> TimeProviderMock { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }
        public Mock<ICurrentUserProvider> CurrentUserProviderMock { get; set; }
        public SimpleTokenProvider SimpleTokenProvider { get; set; }

        #endregion Providers

        public string RandomString => Guid.NewGuid().ToString("n").Substring(0, 8);

        protected InMemoryDatabaseTestBase()
        {
            ServiceCollection = new ServiceCollection();
            InitializeProviders();
            InitializeDbContexts();
            InitializeRepositories();
            InitializeMediatR();
        }

        private void InitializeDbContexts()
        {
            var options = new DbContextOptionsBuilder<ApplicationApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            ArrangeRepositoryContext = new ApplicationApplicationDbContext(options, CurrentUserProviderMock.Object, DateTimeProvider);
            ActRepositoryContext = new ApplicationApplicationDbContext(options, CurrentUserProviderMock.Object, DateTimeProvider);
            AssertRepositoryContext = new ApplicationApplicationDbContext(options, CurrentUserProviderMock.Object, DateTimeProvider);
        }

        private void InitializeMediatR()
        {
            ServiceCollection.RegisterAutoMapper().RegisterMediatR();
            var serviceProvider = ServiceCollection.BuildServiceProvider();
            Mediator = serviceProvider.GetService<IMediator>();
            ServiceCollection.RegisterValidators();
        }

        private void InitializeRepositories()
        {
            UserRepository = new UserRepository(ActRepositoryContext);
            CompanyRepository = new CompanyRepository(ActRepositoryContext);
            OrderRepository = new OrderRepository(ActRepositoryContext);
            ProductRepository = new ProductRepository(ActRepositoryContext);
            UnavailabilityRepository = new UnavailabilityRepository(ActRepositoryContext);
            LogRepositoryMock = new Mock<ILogRepository>();

            ServiceCollection.AddTransient(x => UserRepository);
            ServiceCollection.AddTransient(x => CompanyRepository);
            ServiceCollection.AddTransient(x => LogRepositoryMock.Object);
            ServiceCollection.AddTransient(x => OrderRepository);
            ServiceCollection.AddTransient(x => ProductRepository);
            ServiceCollection.AddTransient(x => UnavailabilityRepository);
        }

        private void InitializeProviders()
        {
            MailerMock = new Mock<IMailer>();
            LoggerMock = new Mock<ILogger>();
            DateTimeProvider = new DateTimeProvider();
            EmailProvider = new EmailProvider(MailerMock.Object);
            SimpleTokenProvider = new SimpleTokenProvider(TimeSpan.FromDays(30), HealthyJuicesConstants.LOCAL_ACCESS_TOKEN_SECRET);
            CurrentUserProviderMock = new Mock<ICurrentUserProvider>();

            ServiceCollection.AddTransient(x => MailerMock.Object);
            ServiceCollection.AddTransient(x => LoggerMock.Object);
            ServiceCollection.AddTransient(x => DateTimeProvider);
            ServiceCollection.AddTransient(x => EmailProvider);
            ServiceCollection.AddTransient(x => SimpleTokenProvider);
        }

        public void Dispose()
        {
            ArrangeRepositoryContext.Dispose();
            AssertRepositoryContext.Dispose();
            ActRepositoryContext.Dispose();
        }
    }
}