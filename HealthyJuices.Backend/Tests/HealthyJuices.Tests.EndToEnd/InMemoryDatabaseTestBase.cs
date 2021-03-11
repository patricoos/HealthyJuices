﻿using System;
using System.Linq;
using HealthyJuices.Application.Auth;
using HealthyJuices.Application.Providers;
using HealthyJuices.Application.Services;
using HealthyJuices.Common;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Services;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Logs.DataAccess;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Domain.Providers;
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

namespace HealthyJuices.Tests.EndToEnd
{
    public abstract class InMemoryDatabaseTestBase : IDisposable
    {
        #region Contexts

        public IDbContext ArrangeRepositoryContext { get; set; }
        public IDbContext ActRepositoryContext { get; set; }
        public IDbContext AssertRepositoryContext { get; set; }


        #endregion Contexts

        #region Repositories

        public IUserRepository UserRepository { get; set; }
        public ICompanyRepository CompanyRepository { get; set; }
        public ILogRepository LogRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IUnavailabilityRepository UnavailabilityRepository { get; set; }

        #endregion Repositories

        #region Services

        public AuthorizationService AuthorizationService { get; set; }
        public CompaniesService CompaniesService { get; set; }
        public OrdersService OrdersService { get; set; }
        public ProductsService ProductsService { get; set; }
        public UnavailabilitiesService UnavailabilitiesService { get; set; }
        public UsersService UsersService { get; set; }

        #endregion Services


        #region Providers
        private readonly Random _random;
        public Mock<IMailer> MailerMock { get; set; }
        public EmailProvider EmailProvider { get; set; }
        public Mock<ITimeProvider> TimeProviderMock { get; set; }
        public ITimeProvider TimeProvider { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }
        public SimpleTokenProvider SimpleTokenProvider { get; set; }


        #endregion Providers

        protected InMemoryDatabaseTestBase()
        {
            InitializeProviders();
            InitializeDbContexts();
            InitializeRepositories();
            InitializeServices();
            SeedGlobalInitData();
            _random = new Random();
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
            UserRepository = new UserRepository(ActRepositoryContext, TimeProvider);
            CompanyRepository = new CompanyRepository(ActRepositoryContext, TimeProvider);
            LogRepository = new LogRepository(ActRepositoryContext, TimeProvider);
            OrderRepository = new OrderRepository(ActRepositoryContext, TimeProvider);
            ProductRepository = new ProductRepository(ActRepositoryContext, TimeProvider);
            UnavailabilityRepository = new UnavailabilityRepository(ActRepositoryContext, TimeProvider);
        }

        private void InitializeProviders()
        {
            MailerMock = new Mock<IMailer>();
            LoggerMock = new Mock<ILogger>();
            TimeProvider = new TimeProvider();
            EmailProvider = new EmailProvider(MailerMock.Object);
            SimpleTokenProvider = new SimpleTokenProvider(TimeSpan.FromDays(30), HealthyJuicesConstants.LOCAL_ACCESS_TOKEN_SECRET);
        }

        private void InitializeServices()
        {
            AuthorizationService = new AuthorizationService(UserRepository, SimpleTokenProvider, TimeProvider, EmailProvider, CompanyRepository);
            CompaniesService = new CompaniesService(CompanyRepository);
            OrdersService = new OrdersService(OrderRepository, UserRepository, ProductRepository, UnavailabilityRepository);
            ProductsService = new ProductsService(ProductRepository);
            UnavailabilitiesService = new UnavailabilitiesService(UnavailabilityRepository);
            UsersService = new UsersService(UserRepository);
        }

        private void SeedGlobalInitData()
        {
        }

        protected int GeneratePositiveRandomNumber(int minValue = 1, int maxValue = int.MaxValue) => _random.Next(minValue, maxValue);
        protected string RandomId => Guid.NewGuid().ToString();

        protected string GenerateRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public void Dispose()
        {
            ArrangeRepositoryContext.Dispose();
            AssertRepositoryContext.Dispose();
            ActRepositoryContext.Dispose();
        }
    }
}