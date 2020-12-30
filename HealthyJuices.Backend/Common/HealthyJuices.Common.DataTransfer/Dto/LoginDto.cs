namespace HealthyJuices.Shared.Dto
{
    public record LoginDto(string Email, string Password);
    public record LoginResponseDto(UserDto User, string AccessToken);

    public record RegisterUserDto(string Email, string Password);

    public record ForgotPasswordDto(string Email);
    public record ResetPasswordDto(string Email, string Password, string Token);
}

