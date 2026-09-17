using Backoffice.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BackofficeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BackofficeDb")));

var app = builder.Build();
