using Backoffice.Api.Data;
using Backoffice.Api.Models;
using Backoffice.Api.Repositories;
using Backoffice.Api.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;




var builder = WebApplication.CreateBuilder(args);

var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);

builder.Services.AddDbContext<BackofficeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BackofficeDb")));

builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddSingleton<IPasswordHasher<AdminUser>, PasswordHasher<AdminUser>>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthorization();

builder.Services.AddSingleton<ILoginRateLimiter, LoginRateLimiter>();

builder.Services.AddControllers();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConnectionMultiplexerFactory = () => Task.FromResult<IConnectionMultiplexer>(redis);
    options.InstanceName = "backoffice:";
});
builder.Services.AddSession();
var app = builder.Build();
app.UseSession();
app.MapControllers();

app.Run();
