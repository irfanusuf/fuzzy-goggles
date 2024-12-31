using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication1.Dependency;
using WebApplication1.Models;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI

builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddSingleton<ITokenService, TokenService>();


builder.Services.AddControllers();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();




app.Run();


