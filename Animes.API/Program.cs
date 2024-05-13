using AnimeAPI.Infrastructure.Data;
using Animes.API.Application;
using Animes.API.Application.Services;
using Animes.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicione os serviços ao contêiner.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AnimeAPI.Infrastructure.Data.AnimeDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAnimeInterface, AnimeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline de solicitação HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
