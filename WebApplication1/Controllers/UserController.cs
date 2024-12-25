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

            await _dbContext.Users.InsertOneAsync(user);

            return Ok(new
            {
                message = "User Created Succesfully!",
                paylaod = new
                {

                    id = user.Id.ToString()
                }
            });
        }

        [HttpGet("getUser/{id}")]

        public async Task<IActionResult> GetUser(string id)
        {

            var user = await _dbContext.Users.Find(user => user.Id.ToString() == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }


            return Ok(new
            {
                message = "user Found ",
                payload = new
                {
                    id = user.Id.ToString(),
                    username = user.Username
                }


            });
        }

        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> DeleteUser(string id)
        {
            var delete = await _dbContext.Users.DeleteOneAsync(user => user.Id.ToString() == id);
            if (delete.DeletedCount == 0)
            {
                return BadRequest(new
                {
                    message = "User not Found may be already deleted",
                });
            }
            return Ok(new
            {
                Message = "Deleted sucessfully",
                payload = new
                {
                    deleteCount = delete.DeletedCount
                }
            });
        }

        [HttpPut("edit/{id}")]
        public async Task <IActionResult> EditUser (string id ,[FromBody] User user) {

            var findUser  = await _dbContext.Users.Find(user =>user.Id.ToString() == id).FirstOrDefaultAsync();

            if(findUser == null){
                return NotFound();
            }

            findUser.Email = user.Email ?? findUser.Email;
            findUser.Username = user.Username ?? findUser.Username; 
            findUser.Phone = user.Phone ?? findUser.Phone;


            await  _dbContext.Users.ReplaceOneAsync(user => user.Id.ToString() == id , findUser);
            return Ok (new {
                message = "edited Succesfully!"
            });
        }

    }
}
