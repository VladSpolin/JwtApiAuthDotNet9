using JwtApiAuthDotNet9.Models;
using JwtApiAuthDotNet9.Models.Dtos;
using JwtApiAuthDotNet9.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

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
            if (user is null) return BadRequest("User is already exists");
        return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginUserDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result is null) return BadRequest("Incorrect login or password");
            Response.Cookies.Append("lala", result.AccessToken);
            return Ok(result);
        }

        [HttpGet("test")]
        [Authorize(Roles ="Admin")]
        public IActionResult OnlyAuthenticated() 
        { 
            return Ok("Authenticated"); 
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null) 
                return BadRequest("Invalid refresh token");

            Response.Cookies.Append("lala", result.AccessToken);
            return Ok(result);
        }
    

    }
}
 