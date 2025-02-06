using AutoMapper;
using JwtApiAuthDotNet9.Models.Dtos;
using JwtApiAuthDotNet9.Models;
using System.Diagnostics.Contracts;

namespace JwtApiAuthDotNet9
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<User, LoginUserDto>();
                config.CreateMap<LoginUserDto, User>();
                config.CreateMap<User, RegisterUserDto>();
                config.CreateMap<RegisterUserDto, User>();
            });
            return mappingConfig;
        }

    }
}
