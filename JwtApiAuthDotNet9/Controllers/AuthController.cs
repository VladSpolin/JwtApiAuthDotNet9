using JwtApiAuthDotNet9.Cryptography.Interfaces;
using JwtApiAuthDotNet9.Models;
using JwtApiAuthDotNet9.Models.Dtos;
using JwtApiAuthDotNet9.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace JwtApiAuthDotNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register( RegisterUserDto request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user == null) return BadRequest("User is already exists");
        return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginUserDto request)
        {
            var token = await _authService.LoginAsync(request);
            if (token == null) return BadRequest("Incorrect login or password");
           
            return Ok(token);
        }

        [HttpGet]
        [Authorize]
        public IActionResult OnlyAuthenticated() 
        { 
            return Ok("Authenticated"); 
        }

    

    }
}
 