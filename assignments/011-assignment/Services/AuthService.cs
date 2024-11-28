using movieReservationSystem.Models;
using movieReservationSystem.Repositories;

namespace movieReservationSystem.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        public User Register(string username, string password)
        {
            if (_userRepository.UsernameExists(username))
            {
                throw new System.Exception("Username already exists.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User { Username = username, PasswordHash = passwordHash };
            return _userRepository.AddUser(user);
        }
    }
}
