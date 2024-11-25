using Carter;
using Microsoft.EntityFrameworkCore;
using MiniApp.Api.Extensions;
using MiniApp.Api.Middlewares;
using MiniApp.Dal.Context;
using MiniApp.Persistence.IProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("AppDb").Replace("~", builder.Environment.ContentRootPath); ;
builder.Services.AddDbContext<ApplicationContext>(x => x.UseSqlite(conn), ServiceLifetime.Transient);

builder.Services.AddAppServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
    context.Database.MigrateAsync().Wait();

    var seedProvider = serviceScope.ServiceProvider.GetRequiredService<ISeedProvider>();
    if (app.Environment.IsDevelopment())
    {
        seedProvider.InitDevelopment().Wait();
    }
    else
    {
        seedProvider.InitProduction().Wait();
    }
}

app.UseHttpsRedirection();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
