using Microsoft.AspNetCore.Mvc;
using movieReservationSystem.Models;
using movieReservationSystem.Services;

namespace movieReservationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var authenticatedUser = _authService.Authenticate(user.Username, user.PasswordHash);
            if (authenticatedUser == null)
            {
                return Unauthorized();
            }
            return Ok(authenticatedUser);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                var newUser = _authService.Register(user.Username, user.PasswordHash);
                return CreatedAtAction(nameof(Login), new { id = newUser.Id }, newUser);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}