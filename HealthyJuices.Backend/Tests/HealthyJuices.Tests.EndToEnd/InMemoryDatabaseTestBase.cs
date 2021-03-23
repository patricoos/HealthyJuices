﻿using System;
using System.Linq;
using HealthyJuices.Application.Auth;
using HealthyJuices.Application.Providers;
using HealthyJuices.Application.Functions;
using HealthyJuices.Application.Functions.Companies.Commands;
using HealthyJuices.Application.Functions.Companies.Queries;
using HealthyJuices.Application.Functions.Orders.Commands;
using HealthyJuices.Application.Functions.Orders.Queries;
using HealthyJuices.Application.Functions.Products.Commands;
using HealthyJuices.Application.Functions.Products.Queries;
using HealthyJuices.Application.Functions.Unavailabilities.Commands;
using HealthyJuices.Application.Functions.Unavailabilities.Queries;
using HealthyJuices.Application.Functions.Users.Commands;
using HealthyJuices.Application.Functions.Users.Queries;
using HealthyJuices.Application.Utils;
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

        public IDbContext ArrangeRepositoryContext { get; set; }
        public IDbContext ActRepositoryContext { get; set; }
        public IDbContext AssertRepositoryContext { get; set; }


        #endregion Contexts

        #region Repositories

        public IUserRepository UserRepository { get; set; }
        public ICompanyWriteRepository CompanyWriteRepository { get; set; }
        public ILogWriteRepository LogWriteRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IUnavailabilityWriteRepository UnavailabilityWriteRepository { get; set; }

        #endregion Repositories

        #region Services
        public ServiceCollection ServiceCollection { get; set; }
        public IMediator Mediator { get; set; }
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
            ServiceCollection = new ServiceCollection();
            InitializeProviders();
            InitializeDbContexts();
            InitializeRepositories();
            InitializeServices();
            InitializeMediatR();
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

        private void InitializeMediatR()
        {
            ServiceCollection.RegisterMediatR();
            var serviceProvider = ServiceCollection.BuildServiceProvider();
            Mediator = serviceProvider.GetService<IMediator>();
            ServiceCollection.RegisterValidators();
        }

        private void InitializeRepositories()
        {
            UserRepository = new UserRepository(ActRepositoryContext);
            CompanyWriteRepository = new CompanyRepository(ActRepositoryContext);
            LogWriteRepository = new LogRepository(ActRepositoryContext);
            OrderRepository = new OrderRepository(ActRepositoryContext);
            ProductRepository = new ProductRepository(ActRepositoryContext);
            UnavailabilityWriteRepository = new UnavailabilityRepository(ActRepositoryContext);

            ServiceCollection.AddTransient(x => UserRepository);
            ServiceCollection.AddTransient(x => CompanyWriteRepository);
            ServiceCollection.AddTransient(x => LogWriteRepository);
            ServiceCollection.AddTransient(x => OrderRepository);
            ServiceCollection.AddTransient(x => ProductRepository);
            ServiceCollection.AddTransient(x => UnavailabilityWriteRepository);
        }

        private void InitializeProviders()
        {
            MailerMock = new Mock<IMailer>();
            LoggerMock = new Mock<ILogger>();
            TimeProvider = new TimeProvider();
            EmailProvider = new EmailProvider(MailerMock.Object);
            SimpleTokenProvider = new SimpleTokenProvider(TimeSpan.FromDays(30), HealthyJuicesConstants.LOCAL_ACCESS_TOKEN_SECRET);

            ServiceCollection.AddTransient(x => MailerMock.Object);
            ServiceCollection.AddTransient(x => LoggerMock.Object);
            ServiceCollection.AddTransient(x => TimeProvider);
            ServiceCollection.AddTransient(x => EmailProvider);
            ServiceCollection.AddTransient(x => SimpleTokenProvider);
        }

        private void InitializeServices()
        {
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