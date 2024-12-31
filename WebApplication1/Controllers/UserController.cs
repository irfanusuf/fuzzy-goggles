using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebApplication1.Dependency;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(MongoDbContext dbContext) : ControllerBase
    {

        private readonly MongoDbContext _dbContext = dbContext;


     [HttpPost("register")]
public async Task<IActionResult> Post([FromBody] User user)
{
    try
    {
  
        var existingUser = await _dbContext.Users.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return BadRequest(new
            {
                message = "A user with this email already exists."
            });
        }

     
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

       
        user.UserDateCreated = DateTime.UtcNow;

     
        await _dbContext.Users.InsertOneAsync(user);

        return Ok(new
        {
            message = "User created successfully!",
            payload = new
            {
                id = user.Id.ToString()
            }
        });
    }
    catch (Exception ex)
    {
       

        return StatusCode(500, new
        {
            message = "Sever Error ",
            error = ex.Message
        });
    }
}

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    try
    {
      
        var user = await _dbContext.Users.Find(u => u.Email == request.Email).FirstOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

      
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes("YourSecretKeyHere"); // Replace with a secure key
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new
        {
            message = "Login successful!",
            token = tokenString
        });
    }
    catch (Exception ex)
    {
        

        return StatusCode(500, new
        {
            message = "An error occurred while processing your request.",
            error = ex.Message
        });
    }
}


public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}


[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request, [FromServices] ITokenService tokenService)
{
    try
    {
        var user = await _dbContext.Users.Find(u => u.Email == request.Email).FirstOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        var token = tokenService.CreateToken(user.Id.ToString(), user.Email, user.Username);

        return Ok(new
        {
            message = "Login successful!",
            token
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            message = "An error occurred while processing your request.",
            error = ex.Message
        });
    }
}


  [HttpGet("getUser/{id}")]
public async Task<IActionResult> GetUser(string id)
{
    try
    {
        var user = await _dbContext.Users.Find(u => u.Id.ToString() == id).FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound(new
            {
                message = "User not found"
            });
        }

        return Ok(new
        {
            message = "User found successfully!",
            payload = new
            {
                id = user.Id.ToString(),
                username = user.Username
            }
        });
    }
    catch (Exception ex)
    {
        

        return StatusCode(500, new
        {
            message = "An error occurred while processing your request.",
            error = ex.Message
        });
    }
}





[HttpDelete("delete/{id}")]
public async Task<IActionResult> DeleteUser(string id)
{
    try
    {
      
        var delete = await _dbContext.Users.DeleteOneAsync(user => user.Id.ToString() == id);

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


  [HttpPut("edit/{id}")]
public async Task<IActionResult> EditUser(string id, [FromBody] User user)
{
    try
    {
      
        var findUser = await _dbContext.Users.Find(u => u.Id.ToString() == id).FirstOrDefaultAsync();

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

    
        await _dbContext.Users.ReplaceOneAsync(u => u.Id.ToString() == id, findUser);

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


    }
}
