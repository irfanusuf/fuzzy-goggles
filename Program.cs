
using WebApplication1.Interfaces;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddCors(Options =>
{
    Options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


});



// dependency injection 
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<ITokenService, TokenService>();



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// middle wares 


app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();



app.Run();

