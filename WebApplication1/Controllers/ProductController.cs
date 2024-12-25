
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("admin/[controller]")]
    [ApiController]
    public class ProductController(ProductService productService) : ControllerBase
    {


        private readonly ProductService _productService = productService;

        [HttpPost("create")]

        public async Task<IActionResult> CreateProduct(Product product)
        {
            await _productService.CreateProductAsync(product);
            return Ok(new
            {
                Message = "Product created successfully!",
                payload = new
                {
                    product.Name,
                    product.Color,
                    product.Qty,
                    product.Size
                }
            });
        }

        
        
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditProduct(string id, Product updatedProduct)
        {
            var product = await _productService.UpdateProductAsync(id, updatedProduct);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found" });
            }

            return Ok(new
            {
                Message = "Product updated successfully!",
                payload = new
                {
                    product.Name,
                    product.Color,
                    product.Qty,
                    product.Size
                }
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound(new { Message = "Product not found" });
            }

            return Ok(new { Message = "Product deleted successfully!" });
        }




         [HttpGet("getAll")]

         public async Task<IActionResult> GetAll(){

            var products = await _productService.GetProductsAsync();

            return Ok(new {message = "all products are here "});

         }



    }
}   
