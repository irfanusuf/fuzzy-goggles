using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase    // inheritance with controller base 
    {

     [HttpPost("create")]

     public async Task <IActionResult> Create (){

        return Ok();
     }
       

    }
}
