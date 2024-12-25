// using WebApplication1.Dependency;
// using WebApplication1.Models;
// using WebApplication1.Services;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddSingleton<MongoDbContext>();

// // Register ProductService for dependency injection
// builder.Services.AddScoped<ProductService>();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// // Route for creating a product
// app.MapPost("/admin/create/product", async (Product product, ProductService productService) =>
// {
//     await productService.CreateProductAsync(product);
//     return Results.Ok(new
//     {
//         Message = "Product created successfully!",
//         payload = new
//         {
//             product.Name,
//             product.Color,
//             product.Qty,
//             product.Size
//         }
//     });
// });

// // Route for updating a product
// app.MapPut("/admin/edit/product/{id}", async (string id, Product updatedProduct, ProductService productService) =>
// {
//     var product = await productService.UpdateProductAsync(id, updatedProduct);
//     if (product == null)
//     {
//         return Results.NotFound(new { Message = "Product not found" });
//     }

//     return Results.Ok(new
//     {
//         Message = "Product updated successfully!",
//         payload = new
//         {
//             product.Name,
//             product.Color,
//             product.Qty,
//             product.Size
//         }
//     });
// });

// // Route for deleting a product
// app.MapDelete("/admin/delete/product/{id}", async (string id, ProductService productService) =>
// {
//     var success = await productService.DeleteProductAsync(id);
//     if (!success)
//     {
//         return Results.NotFound(new { Message = "Product not found" });
//     }

//     return Results.Ok(new { Message = "Product deleted successfully!" });
// });

// app.Run();






// app.MapGet("/", () => "Hello World From The Server !");

// app.MapPost("/add", (int a, int b) => Results.Ok(a + b));

// app.MapPost("/users/register", async (User user, MongoDbContext dbContext) =>
// {
//     await dbContext.Users.InsertOneAsync(user);
//     return Results.Ok(new
//     {
//         Message = "User created successfully",
//         payload = new
//         {
//             Id = user.Id.ToString(),  // Convert ObjectId to string
//             user.Username,
//             user.Email,
//             user.Phone
//         }
//     });
// });




// app.MapGet("/users/get/{id}", async (string id, MongoDbContext dbContext) =>
// {
//     var user = await dbContext.Users.Find(u => u.Id.ToString() == id).FirstOrDefaultAsync();
//     if (user == null)
//     {
//         return Results.NotFound(new { Message = "User not found" });
//     }
//     return Results.Ok(new
//     {
//         Message = "User retrieved successfully",
//         payload = new
//         {
//             user.Username,
//             user.Email,
//             user.Phone

//         }
//     });
// });


// app.MapPost("/admin/create/product", async (Product product, MongoDbContext dbContext) =>
// {

//     await dbContext.Products.InsertOneAsync(product);
//     return Results.Ok(new
//     {
//         Message = "Product created Succesfully!",
//         payload = new
//         {
//             product.Name,
//             product.Color,
//             product.Qty,
//             product.Size

//         }
//     });
// });


// app.MapPut("/admin/edit/product/{id}", async (string id, Product updatedProduct, MongoDbContext dbContext) =>
// {
//     // Find the product by its Id
//     var product = await dbContext.Products.Find(p => p.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();

//     if (product == null)
//     {
//         return Results.NotFound(new { Message = "Product not found" });
//     }

//     // Update the product's details (only the fields that are provided)
//     product.Name = updatedProduct.Name ?? product.Name;
//     product.Color = updatedProduct.Color ?? product.Color;
//     product.Qty = updatedProduct.Qty != 0 ? updatedProduct.Qty : product.Qty;
//     product.Size = updatedProduct.Size ?? product.Size;

//     // Update the product in the database
//     await dbContext.Products.ReplaceOneAsync(p => p.Id == ObjectId.Parse(id), product);

//     return Results.Ok(new
//     {    
//         Message = "Product updated successfully!",
//         payload = new
//         {
//             product.Name,
//             product.Color,
//             product.Qty,
//             product.Size
//         }
//     });
// });





// app.MapDelete("/admin/delete/product/{id}", async (string id, MongoDbContext dbContext) =>
// {
//     // Find and delete the product by its Id
//     var result = await dbContext.Products.DeleteOneAsync(p => p.Id == ObjectId.Parse(id));

//     if (result.DeletedCount == 0)
//     {
//         return Results.NotFound(new { Message = "Product not found" });
//     }

//     return Results.Ok(new { Message = "Product deleted successfully!" });
// });
