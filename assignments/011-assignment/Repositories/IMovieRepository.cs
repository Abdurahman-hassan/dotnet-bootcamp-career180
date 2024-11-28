using movieReservationSystem.Models;
using System.Collections.Generic;

namespace movieReservationSystem.Repositories
{
    public interface IMovieRepository
    {
        List<Movie> GetAllMovies();
        Movie GetMovieById(string id);
        Movie AddMovie(Movie movie);
        Movie UpdateMovie(Movie movie);
        void DeleteMovie(string id);
    }
}