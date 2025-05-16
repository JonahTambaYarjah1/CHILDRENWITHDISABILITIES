using Microsoft.AspNetCore.Mvc;
using ChildrenWithDisabilitiesAPI.DTOs;
using ChildrenWithDisabilitiesAPI.Models;
using ChildrenWithDisabilitiesAPI.Services;

namespace ChildrenWithDisabilitiesAPI.Controllers
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var user = new User
            {
                Full_Name = dto.Full_Name,
                Email = dto.Email,
                Password = dto.Password
            };

            var result = await _authService.Register(user);
            return result ? Ok("User registered.") : BadRequest("Email already exists.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _authService.Login(dto.Email, dto.Password);
            if (user == null) return Unauthorized("Invalid credentials.");
            return Ok(new { user.Full_Name, user.Email });
        }
    }
}
