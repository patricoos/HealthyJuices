using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HealthyJuices.Application.Functions.Auth.Commands;
using HealthyJuices.Application.Providers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Domain.Providers;
using HealthyJuices.Shared.Enums;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace HealthyJuices.Application.Tests.Commands.Auth
{
    public class RegisterUserHandlerTests
    {
        [Fact]
        public async Task Handler_should_throw_exception_on_null_company()
        {
            //  arrange
            var repositoryMock = new Mock<IUserRepository>();
            var companyMock = new Mock<ICompanyRepository>();
            var timeProvicerMock = new Mock<IDateTimeProvider>();
            var mailerMock = new Mock<IMailer>();
            var emailProvicerMock = new Mock<EmailProvider>(mailerMock.Object);

            repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync((User)null);

            var handler = new Register.Handler(repositoryMock.Object, emailProvicerMock.Object, timeProvicerMock.Object, companyMock.Object);

            var command = new Register.Command("email", "password", "firstName", "lastName", "testCompanyId");

            // act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);
            var exception = await Assert.ThrowsAsync<BadRequestException>(act);
            
            //assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Company not found");
        }
    }
}