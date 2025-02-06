using AutoMapper;
using JwtApiAuthDotNet9;
using JwtApiAuthDotNet9.Cryptography;
using JwtApiAuthDotNet9.Cryptography.Interfaces;
using JwtApiAuthDotNet9.Extensions;
using JwtApiAuthDotNet9.Services.Implementations;
using JwtApiAuthDotNet9.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IAuthService,  AuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) 
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
