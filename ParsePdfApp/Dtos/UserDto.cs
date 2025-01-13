namespace ParsePdfApp.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; }
    }
}
