using Microsoft.AspNetCore.Mvc;
using movieReservationSystem.Models;
using movieReservationSystem.Services;
using System.Collections.Generic;

namespace movieReservationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/movie
        [HttpGet]
        public ActionResult<List<Movie>> Get()
        {
            var movies = _movieService.GetAllMovies();
            if (movies == null || movies.Count == 0)
            {
                return NoContent();
            }
            return Ok(movies);
        }

        // GET: api/movie/{id}
        [HttpGet("{id:length(24)}", Name = "GetMovie")]
        public ActionResult<Movie> Get(string id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        // POST: api/movie
        [HttpPost]
        public ActionResult<Movie> Create(Movie movie)
        {
            _movieService.AddMovie(movie);
            return CreatedAtRoute("GetMovie", new { id = movie.Id.ToString() }, movie);
        }

        // POST: api/movie/bulk
        [HttpPost("bulk")]
        public ActionResult<List<Movie>> CreateBulk([FromBody] List<Movie> movies)
        {
            if (movies == null || movies.Count == 0)
            {
                return BadRequest("No movies provided.");
            }

            var createdMovies = new List<Movie>();
            foreach (var movie in movies)
            {
                var createdMovie = _movieService.AddMovie(movie);
                createdMovies.Add(createdMovie);
            }

            return Ok(createdMovies);
        }

        // PUT: api/movie/{id}
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Movie movieIn)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            _movieService.UpdateMovie(movieIn);
            return NoContent();
        }

        // DELETE: api/movie/{id}
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            _movieService.DeleteMovie(movie.Id);
            return NoContent();
        }
    }
}