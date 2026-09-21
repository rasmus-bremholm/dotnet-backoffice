using Backoffice.Api.Data;
using Backoffice.Api.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<BackofficeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BackofficeDb")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();

app.Run();
