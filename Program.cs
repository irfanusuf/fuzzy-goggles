using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}else{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/" , ()=> "heelo world");

app.MapGet("/user/{userId}" ,  (string userId)=> "hello userr with id ");

app.MapPost("/user/register" , (User user)=> "jssddsfdfdsf");

// app.MapPost("/admin/create/product" , (Product product , User user)=>"kuch bhi " );

app.MapPut("/product/edit/{id}" , (string id ,Product product) => "hkndsahfuk" );

app.MapDelete("/user/delete/{id}" , (string id ) => "hkndsahfuk" );

app.Run();

