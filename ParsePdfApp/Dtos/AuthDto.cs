namespace ParsePdfApp.Dtos
{
    public class AuthUserDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public UserDto User { get; set; }
    }
}
