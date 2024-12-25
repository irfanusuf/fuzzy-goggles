using MongoDB.Driver;
using WebApplication1.Models;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dependency injection 

builder.Services.AddSingleton<MongoDbService>();



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapGet("/", () => "heelo world");

app.MapGet("/user/{userId}", async (string userId, MongoDbService dbService) =>
 {

     var existinguser = await dbService.Users.Find(element => element.Id.ToString() == userId).FirstOrDefaultAsync();


     if (existinguser == null)
     {
         return Results.BadRequest(new
         {
             message = "User Not Found!"
         });
     }

     return Results.Ok(new
     {
         message = "One User Found ",
         username = existinguser.Username


     });

 });

app.MapPost("/user/register", async (User user, MongoDbService dbService) =>
{

    var existinguser = await dbService.Users.Find(element => element.Email == user.Email).FirstOrDefaultAsync();

    if (existinguser != null)
    {

        return Results.BadRequest(new { message = "User already Exists" });

    }

    await dbService.Users.InsertOneAsync(user);

    return Results.Ok(new { message = "User Created Succesfully" });


});




app.Run();

