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
