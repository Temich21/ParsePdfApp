using ParsePdfApp.Models;

namespace ParsePdfApp.Repositories
{
    public class UserRepository
    {
        private readonly List<User> _users;

        public UserRepository()
        {
            // Инициализируем список пользователей здесь, чтобы он не требовал регистрации в DI контейнере
            _users = new List<User>
            {
                new User {
                    Id = Guid.Parse("9339124a-c1e7-4df0-9910-d761bf87e630"),
                    Name = "John Doe",
                    Email = "rahmat97@mail.ru",
                    Password = "$2a$13$BK0EUlsZnKR08kiYh0drBusXYwbv2ZBA1xGsDuMMbxebzjMqOz0s."
                },
            };
        }

        // Метод для создания нового пользователя
        public Task<User> CreateAsync(User user)
        {
            _users.Add(user);
            return Task.FromResult(user); // Возвращаем Task, чтобы соответствовать асинхронной сигнатуре
        }

        // Метод для поиска пользователя по ID
        public Task<User?> FindAsync(Guid userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            return Task.FromResult(user);
        }

        // Метод для поиска пользователя по email
        public Task<User?> FindByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        // Метод для удаления пользователя
        public Task<bool> DeleteAsync(Guid userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                _users.Remove(user);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        // Метод для обновления информации о пользователе
        public Task<User?> UpdateAsync(User updatedUser)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (existingUser == null)
            {
                return Task.FromResult<User?>(null);
            }

            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.Password = updatedUser.Password;

            return Task.FromResult(existingUser);
        }

        // Метод для получения всех пользователей
        public Task<List<User>> GetAllAsync()
        {
            return Task.FromResult(_users);
        }
    }
}