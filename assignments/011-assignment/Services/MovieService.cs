using movieReservationSystem.Models;
using movieReservationSystem.Repositories;
using System.Collections.Generic;

namespace movieReservationSystem.Services
{
    public class MovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public List<Movie> GetAllMovies()
        {
            return _movieRepository.GetAllMovies();
        }

        public Movie GetMovieById(string id)
        {
            return _movieRepository.GetMovieById(id);
        }

        public Movie AddMovie(Movie movie)
        {
            return _movieRepository.AddMovie(movie);
        }

        public Movie UpdateMovie(Movie movie)
        {
            return _movieRepository.UpdateMovie(movie);
        }

        public void DeleteMovie(string id)
        {
            _movieRepository.DeleteMovie(id);
        }
    }
}