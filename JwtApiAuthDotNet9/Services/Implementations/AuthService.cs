using AutoMapper;
using Azure.Core;
using JwtApiAuthDotNet9.Cryptography.Interfaces;
using JwtApiAuthDotNet9.Models;
using JwtApiAuthDotNet9.Models.Dtos;
using JwtApiAuthDotNet9.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace JwtApiAuthDotNet9.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;
        public AuthService(ApplicationDatabaseContext context, IJwtProvider jwtProvider, IMapper mapper)
        {
            _context = context;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
        }
       

        public async Task<User> RegisterAsync(RegisterUserDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email)) return null;
            var user = _mapper.Map<User>(userDto);
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, userDto.Password);
            user.PasswordHash = hashedPassword;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<TokenResponseDto> LoginAsync(LoginUserDto userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (user is null)
                return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userDto.Password) == PasswordVerificationResult.Failed)
                return null;

            return await CreateTokenResponse(user);
        }


        public async Task<TokenResponseDto> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

            if (user is null) 
                return null;

            return await CreateTokenResponse(user);
        }




        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)

        {
            var user = await _context.Users.FindAsync(userId);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            return user;
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User? user)
        {
            return new TokenResponseDto
            {
                AccessToken = _jwtProvider.GenerateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user),
            };
        }



        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = _jwtProvider.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        
    }
}
