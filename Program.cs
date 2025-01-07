
using CloudinaryDotNet;
using dotenv.net;
using WebApplication1.Interfaces;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);


// load env file 
try
{
    DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
}
catch (Exception ex)
{
    throw new InvalidOperationException("Failed to load .env file.", ex);
}


// connect with Cloudinary and register it

var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");


if (string.IsNullOrEmpty(cloudinaryUrl))
{
    throw new InvalidOperationException("CLOUDINARY_URL environment variable is not set.");
}

Cloudinary cloudinary = new(cloudinaryUrl) { Api = { Secure = true } };





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
builder.Services.AddSingleton<ICloudinaryService, CloudinaryService>();
builder.Services.AddSingleton(cloudinary);





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

