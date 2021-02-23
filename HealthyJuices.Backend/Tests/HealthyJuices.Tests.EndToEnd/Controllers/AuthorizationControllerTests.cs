using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HealthyJuices.Api.Controllers;
using HealthyJuices.Common;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using HealthyJuices.Tests.EndToEnd.SeedTestData;
using Moq;
using Xunit;

namespace HealthyJuices.Tests.EndToEnd.Controllers
{
    public class AuthorizationControllerTests : InMemoryDatabaseTestBase
    {
        [Fact]
        public async Task Can_register_as_Customer_and_send_email()
        {
            // arrange
            var company = CompanyBuilder.Create().WithName("test company").Build(ArrangeRepositoryContext);
            var dto = new RegisterUserDto("test@email.com", "test pass", "", "", company.Id);

            var controller = new AuthorizationController(AuthorizationService);

            // act
            await controller.RegisterAsync(dto);

            // assert
            var subject = AssertRepositoryContext.Users.FirstOrDefault();
            subject.Should().NotBeNull();
            subject.Roles.Should().Be(UserRole.Customer);

            subject.Email.Should().Be(dto.Email);
            subject.Password.Should().NotBeNullOrWhiteSpace();
            subject.PasswordSalt.Should().NotBeNullOrWhiteSpace();
            subject.IsActive.Should().BeFalse();
            subject.IsRemoved.Should().BeFalse();
            subject.PermissionsToken.Should().NotBeNullOrEmpty();
            subject.PermissionsTokenExpiration.Should().BeAfter(DateTime.Now);
            subject.PermissionsTokenExpiration.Should().BeBefore(DateTime.Now.AddDays(2));

            MailerMock.Verify(mock => mock.SendAsync(It.Is<string>(x => x == dto.Email), It.IsAny<string>(), It.Is<string>(x => x.Contains(subject.PermissionsToken)), false), Times.Once);
            MailerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Can_Login_as_Customer()
        {
            // arrange
            var user = UserBuilder.Create()
                .WithRole(UserRole.Customer)
                .WithEmail(HealthyJuicesConstants.DEFAULT_USER_LOGIN)
                .Active()
                .WithPassword(HealthyJuicesConstants.DEFAULT_USER_PASSWORD)
                .Build(ArrangeRepositoryContext);

            var dto = new LoginDto(HealthyJuicesConstants.DEFAULT_USER_LOGIN, HealthyJuicesConstants.DEFAULT_USER_PASSWORD);

            var controller = new AuthorizationController(AuthorizationService);

            // act
            var result = await controller.LoginAsync(dto);

            // assert
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.User.Should().NotBeNull();
            result.User.Email.Should().Be(dto.Email);

            var subject = AssertRepositoryContext.Users.FirstOrDefault();
            subject.Should().NotBeNull();
            subject.Roles.Should().Be(UserRole.Customer);

            subject.Email.Should().Be(dto.Email);
            subject.Password.Should().NotBeNullOrWhiteSpace();
            subject.PasswordSalt.Should().NotBeNullOrWhiteSpace();
            subject.IsActive.Should().BeTrue();
            subject.IsRemoved.Should().BeFalse();
        }

        [Fact]
        public async Task ForgotPassword_Should_save_new_token_and_send_email()
        {
            // arrange
            var salt = PasswordHelper.GenerateSalt();

            var user = UserBuilder.Create().WithRole(UserRole.Customer).WithEmail(HealthyJuicesConstants.DEFAULT_USER_LOGIN).WithPassword(HealthyJuicesConstants.DEFAULT_USER_PASSWORD).Build(ArrangeRepositoryContext);

            var controller = new AuthorizationController(AuthorizationService);
            var dto = new ForgotPasswordDto(HealthyJuicesConstants.DEFAULT_USER_LOGIN);

            // act
            await controller.ForgotPasswordAsync(dto);

            // assert
            var subject = AssertRepositoryContext.Users.FirstOrDefault();
            subject.Should().NotBeNull();
            subject.PermissionsToken.Should().NotBeNullOrEmpty();
            subject.PermissionsTokenExpiration.Should().BeAfter(DateTime.Now);
            subject.PermissionsTokenExpiration.Should().BeBefore(DateTime.Now.AddDays(2));

            MailerMock.Verify(mock => mock.SendAsync(It.Is<string>(x => x == HealthyJuicesConstants.DEFAULT_USER_LOGIN), It.IsAny<string>(), It.Is<string>(x => x.Contains(subject.PermissionsToken)), false), Times.Once);
            MailerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ConfirmEmailRegister_Should_activate_user_and_reset_pemisiontoken()
        {
            // arrange
            var user = UserBuilder.Create()
                .WithEmail(HealthyJuicesConstants.DEFAULT_USER_LOGIN)
                .WithPermissionsToken(GenerateRandomString(), DateTime.Today.AddDays(1))
                .Build(ArrangeRepositoryContext);

            var controller = new AuthorizationController(AuthorizationService);

            // act
            await controller.ConfirmRegisterAsync(HealthyJuicesConstants.DEFAULT_USER_LOGIN, user.PermissionsToken);

            // assert
            var subject = AssertRepositoryContext.Users.FirstOrDefault();
            subject.Should().NotBeNull();
            subject.IsActive.Should().BeTrue();
            subject.PermissionsToken.Should().BeNullOrEmpty();
            subject.PermissionsTokenExpiration.Should().BeNull();
        }

        [Fact]
        public async Task ResetPassword_Should_change_password_and_reset_pemisiontoken()
        {
            // arrange
            var user = UserBuilder.Create()
                .WithEmail(HealthyJuicesConstants.DEFAULT_USER_LOGIN)
                .WithPassword(GenerateRandomString())
                .WithPermissionsToken(GenerateRandomString(), DateTime.Today.AddDays(1))
                .Active()
                .Build(ArrangeRepositoryContext);

            var controller = new AuthorizationController(AuthorizationService);
            var dto = new ResetPasswordDto(HealthyJuicesConstants.DEFAULT_USER_LOGIN, "test new pass", user.PermissionsToken);

            // act
            await controller.ResetPasswordAsync(dto);

            // assert
            var subject = AssertRepositoryContext.Users.FirstOrDefault();
            subject.Should().NotBeNull();
            subject.PasswordSalt.Should().NotBe(user.PasswordSalt);
            subject.PasswordSalt.Should().NotBe(user.Password);
            subject.IsActive.Should().BeTrue();
            subject.PermissionsToken.Should().BeNullOrEmpty();
            subject.PermissionsTokenExpiration.Should().BeBefore(DateTime.Now);

            MailerMock.VerifyNoOtherCalls();
        }
    }
}