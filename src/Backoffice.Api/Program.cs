using Backoffice.Api.Data;
using Backoffice.Api.Models;
using Backoffice.Api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<BackofficeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BackofficeDb")));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddSingleton<IPasswordHasher<AdminUser>, PasswordHasher<AdminUser>>();

builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();

app.Run();
