namespace HealthyJuices.Shared.Dto.Auth
{
    public record LoginResponseDto
    {
        public UserDto User { get; init; }
        public string AccessToken { get; init; }
        public string RefreshToken { get; set; }

        public LoginResponseDto() { }

        public LoginResponseDto(UserDto user, string accessToken)
        {
            this.User = user;
            this.AccessToken = accessToken;
        }
    }
}