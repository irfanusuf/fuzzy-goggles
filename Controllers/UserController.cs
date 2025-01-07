
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;
using MongoDB.Driver;
using WebApplication1.Interfaces;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(MongoDbService dbService, ICloudinaryService cloudinary) : ControllerBase    // inheritance with controller base 
    {

        private readonly MongoDbService _dbservice = dbService;
        private readonly ICloudinaryService _cloudinary = cloudinary;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                var existingUser = await _dbservice.Users.Find(u => u.Email == user.Email).FirstOrDefaultAsync();

                if (existingUser != null)
                {
                    return BadRequest(new      //400
                    {
                        message = "A user with this email already exists."
                    });
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);    // encryption 

                user.DateCreated = DateTime.UtcNow;     // universal time code 


                await _dbservice.Users.InsertOneAsync(user);

                return Ok(new
                {
                    message = "User created successfully!",
                    payload = new
                    {
                        id = user.Id.ToString()
                    }
                });
            }
            catch (Exception error)
            {

                Console.WriteLine(error.Message);
                return StatusCode(500, new
                {
                    message = "Server Error , try Again after Sometime ",

                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login request, ITokenService tokenService)
        {

            try
            {
                var user = await _dbservice.Users.Find(u => u.Email == request.Email).FirstOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest(new      //400
                    {
                        message = "No User Found with this Email!"
                    });
                }

                var verify = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);


                if (verify)
                {
                    // json tokeen create kerna hai 
                    var token = tokenService.CreateToken(user.Id.ToString(), user.Email, user.Username);


                    return Ok(new      //200
                    {
                        message = "Logged In succesFully!",
                        payload = token
                    });
                }
                else
                {
                    return BadRequest(new      //200
                    {
                        message = "Incorrect PassWord! "
                    });
                }




            }
            catch (Exception error)
            {

                Console.WriteLine(error.Message);
                return StatusCode(500, new
                {
                    message = "Server Error , Try Again after Sometime ",

                });

            }
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditUser(string id, [FromBody] User user)
        {
            try
            {

                var findUser = await _dbservice.Users.Find(u => u.Id.ToString() == id).FirstOrDefaultAsync();

                if (findUser == null)
                {
                    return NotFound(new
                    {
                        message = "User not found."
                    });
                }


                findUser.Email = user.Email ?? findUser.Email;
                findUser.Username = user.Username ?? findUser.Username;
                findUser.Phone = user.Phone ?? findUser.Phone;


                await _dbservice.Users.ReplaceOneAsync(u => u.Id.ToString() == id, findUser);

                return Ok(new
                {
                    message = "User edited successfully!"
                });
            }
            catch (Exception ex)
            {


                return StatusCode(500, new
                {
                    message = "Server Error | 500",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {

                var delete = await _dbservice.Users.DeleteOneAsync(user => user.Id.ToString() == id);

                if (delete.DeletedCount == 0)
                {
                    return BadRequest(new
                    {
                        message = "User not found or may have already been deleted."
                    });
                }

                return Ok(new
                {
                    message = "User deleted successfully!",
                    payload = new
                    {
                        deleteCount = delete.DeletedCount
                    }
                });
            }
            catch (Exception ex)
            {


                return StatusCode(500, new
                {
                    message = "Server Error!",
                    error = ex.Message
                });
            }
        }



        [HttpPost("/upload/profile/{id}")]
        public async Task<IActionResult> Upload(string id, IFormFile file)
        {
            try
            {

                var findUser = await _dbservice.Users.Find(u => u.Id.ToString() == id).FirstOrDefaultAsync();
                if (findUser == null)
                {
                    return NotFound(new { message = "User not found." });
                }


                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "Invalid file uploaded." });
                }

                var uploadURL = _cloudinary.UploadImageAsync(file);

                findUser.ProfilePictureUrl = uploadURL.Result.ToString();


                await _dbservice.Users.ReplaceOneAsync(u => u.Id.ToString() == id, findUser);

                return Ok(new
                {
                    message = "File uploaded successfully.",
                    imageUrl = uploadURL.Result.ToString()
                });
            }

            catch (Exception error)
            {
                return StatusCode(500, new { message = $"Server Error: {error.Message}" });
            }
        }


        [HttpPost("/sendmail")]

        public async Task<IActionResult> SendMail(string email)
        {

            var emailService = new EmailService();
            await emailService.SendEmailAsync(
                "recipient@example.com",
                "Test Email",
                "<h1>Hello World!</h1><p>This is a test email.</p>",
                true
            );
                return Ok();
        }

    }
}