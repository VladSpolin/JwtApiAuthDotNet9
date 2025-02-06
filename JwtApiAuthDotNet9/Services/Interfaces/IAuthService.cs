using JwtApiAuthDotNet9.Models;
using JwtApiAuthDotNet9.Models.Dtos;

namespace JwtApiAuthDotNet9.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterUserDto userDto);
        Task<string> LoginAsync(LoginUserDto userDto);
    }
}
