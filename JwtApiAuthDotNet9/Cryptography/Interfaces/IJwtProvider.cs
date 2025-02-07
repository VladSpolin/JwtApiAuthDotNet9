using JwtApiAuthDotNet9.Models;

namespace JwtApiAuthDotNet9.Cryptography.Interfaces
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
        public string GenerateRefreshToken();

    }
}
