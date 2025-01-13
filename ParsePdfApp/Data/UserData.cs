using ParsePdfApp.Models;

namespace ParsePdfApp.Data
{
    public class UserData
    {
        public List<User> Users { get; set; } = new List<User>
        {
            new() {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "test@test.test",
                Password = "$2a$13$uBeh5jG8XgK85q/Hyus97uiddAmpiCs9X.ZztKrKL35skDHtpd7YS"
            },
        };
    }
}
