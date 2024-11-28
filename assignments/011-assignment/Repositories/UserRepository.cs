using MongoDB.Driver;
using movieReservationSystem.Models;
using movieReservationSystem.Utils;
using System.Collections.Generic;
using System.Linq;

namespace movieReservationSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbContext dbContext)
        {
            _users = dbContext.Users;
        }

        public List<User> GetAllUsers() =>
            _users.Find(user => true).ToList();

        public User GetUserById(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User GetUserByUsername(string username) =>
            _users.Find<User>(user => user.Username == username).FirstOrDefault();

        public User GetUserByUsernameAndPassword(string username, string password) =>
            _users.Find<User>(user => user.Username == username && user.PasswordHash == password).FirstOrDefault();

        public bool UsernameExists(string username) =>
            _users.Find<User>(user => user.Username == username).Any();

        public User AddUser(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public User UpdateUser(User userIn)
        {
            _users.ReplaceOne(user => user.Id == userIn.Id, userIn);
            return userIn;
        }

        public void DeleteUser(string id) =>
            _users.DeleteOne(user => user.Id == id);
    }
}