﻿namespace JwtApiAuthDotNet9.Models.Dtos
{
    public class RefreshTokenRequestDto
    {
        public Guid UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
