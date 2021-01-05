namespace HealthyJuices.Shared.Dto
{
    public record LoginDto(string Email, string Password);

    public record RegisterUserDto(string Email, string Password, string FirstName, string LastName, long CompanyId);

    public record ForgotPasswordDto(string Email);
    public record ResetPasswordDto(string Email, string Password, string Token);
}

