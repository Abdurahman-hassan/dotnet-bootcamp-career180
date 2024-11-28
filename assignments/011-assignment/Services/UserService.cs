using movieReservationSystem.Models;
using movieReservationSystem.Repositories;
using System.Collections.Generic;

namespace movieReservationSystem.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserById(string id)
        {
            return _userRepository.GetUserById(id);
        }

        public User AddUser(User user)
        {
            return _userRepository.AddUser(user);
        }

        public User UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }

        public void DeleteUser(string id)
        {
            _userRepository.DeleteUser(id);
        }
    }
}