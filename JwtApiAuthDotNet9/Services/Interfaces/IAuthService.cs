using JwtApiAuthDotNet9.Models;
using JwtApiAuthDotNet9.Models.Dtos;

namespace JwtApiAuthDotNet9.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterUserDto userDto);
        Task<TokenResponseDto> LoginAsync(LoginUserDto userDto);
        Task<TokenResponseDto> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}
