using movieReservationSystem.Models;
using System.Collections.Generic;

namespace movieReservationSystem.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserById(string id);
        User GetUserByUsername(string username);
        User GetUserByUsernameAndPassword(string username, string password);
        bool UsernameExists(string username);
        User AddUser(User user);
        User UpdateUser(User user);
        void DeleteUser(string id);
    }
}