using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Adding  services to the container
builder.Services.AddControllers();

// Adding CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", builder =>
    {
        builder.WithOrigins("http://localhost:5012") 
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseRouting();

// Enabling CORS
app.UseCors("AllowBlazorApp");

app.MapControllers();

app.Run();
