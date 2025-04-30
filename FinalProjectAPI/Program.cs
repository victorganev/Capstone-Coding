using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

// Map controllers
app.MapControllers();

app.Run();
