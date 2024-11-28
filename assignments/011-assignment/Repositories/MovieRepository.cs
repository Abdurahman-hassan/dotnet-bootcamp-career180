using MongoDB.Driver;
using movieReservationSystem.Models;
using movieReservationSystem.Utils;
using System.Collections.Generic;
using System.Linq;

namespace movieReservationSystem.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoCollection<Movie> _movies;

        public MovieRepository(MongoDbContext dbContext)
        {
            _movies = dbContext.Movies;
        }

        public List<Movie> GetAllMovies() =>
            _movies.Find(movie => true).ToList();

        public Movie GetMovieById(string id) =>
            _movies.Find<Movie>(movie => movie.Id == id).FirstOrDefault();

        public Movie AddMovie(Movie movie)
        {
            _movies.InsertOne(movie);
            return movie;
        }

        public Movie UpdateMovie(Movie movieIn)
        {
            _movies.ReplaceOne(movie => movie.Id == movieIn.Id, movieIn);
            return movieIn;
        }

        public void DeleteMovie(string id) =>
            _movies.DeleteOne(movie => movie.Id == id);
    }
}