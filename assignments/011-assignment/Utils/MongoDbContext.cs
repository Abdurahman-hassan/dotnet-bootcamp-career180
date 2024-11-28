using MongoDB.Driver;

namespace movieReservationSystem.Utils
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Models.User> Users => _database.GetCollection<Models.User>("Users");
        public IMongoCollection<Models.Movie> Movies => _database.GetCollection<Models.Movie>("Movies");
        public IMongoCollection<Models.Reservation> Reservations => _database.GetCollection<Models.Reservation>("Reservations");
    }
}