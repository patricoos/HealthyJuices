using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HealthyJuices.Application.Functions.Auth.Commands;
using HealthyJuices.Domain.Models.Users.DataAccess;
using Moq;
using Xunit;

namespace HealthyJuices.Application.Tests.Commands.Auth
{
    public class RegisterUserValidatorTests
    {

        [Fact]
        public async Task Validator_should_throw_exception_on_invalid_email_format()
        {
            //  arrange
            var repositoryMock = new Mock<IUserRepository>();
            var validator = new Register.Validator(repositoryMock.Object);

            var command = new Register.Command("email", "password", "firstName", "lastName", "testCompanyId");

            // act
            var result = await validator.ValidateAsync(command);

            //assert
            result.IsValid.Should().BeFalse();
            result.Errors.FirstOrDefault(x => x.PropertyName == nameof(Register.Command.Email)).Should().NotBeNull();
        }
    }
}