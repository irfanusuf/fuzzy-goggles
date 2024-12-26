using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;
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

app.MapGet("/", () => "hello world!");

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

app.MapPut("/user/edit/{id}", async (string id, User updateduser, MongoDbService dbService) =>
{


    var finduser = await dbService.Users.Find(u => u.Id.ToString() == id).FirstOrDefaultAsync();

    if (finduser == null)
    {
        return Results.NotFound(new { message = "User not Found!" });
    }
    // product.Name = updatedProduct.Name ?? product.Name;
    finduser.Username = updateduser.Username ?? finduser.Username;
    finduser.Email = updateduser.Email ?? finduser.Email;
    finduser.Password = updateduser.Password ?? finduser.Password;
    // await dbService.Users.ReplaceOneAsync(u => u.Id == ObjectId.Parse(id) , finduser  );
    await dbService.Users.ReplaceOneAsync(u => u.Id.ToString() == id, finduser);

    return Results.Ok(new
    {

        message = "user updated Succesfully",
        payload = new
        {
            username = finduser.Username,
            email = finduser.Email

        }

    });


});


app.MapDelete("/user/delete/{id}" , async(string id , MongoDbService dbService) =>{

 var finduser = await dbService.Users.Find(u => u.Id.ToString() == id).FirstOrDefaultAsync();

    if (finduser == null)
    {
        return Results.NotFound(new { message = "User not Found!" });
    }

await dbService.Users.DeleteOneAsync(u => u.Id.ToString() == id);


return Results.Ok(

new{
    message = "Deleted Succesfully!"
}

);

});

app.Run();

