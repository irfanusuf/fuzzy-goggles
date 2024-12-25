using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication1.Dependency;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ProductService(MongoDbContext dbContext)
    {
        private readonly MongoDbContext _dbContext = dbContext;

        // private readonly MongoDbContext _dbContext 
        // public ProductService(MongoDbContext dbContext)  
        // {
        //     _dbContext = dbContext;
        // }

        // Create a new product
        public async Task CreateProductAsync(Product product)
        {
            await _dbContext.Products.InsertOneAsync(product);
        }

        // Update an existing product
        public async Task<Product?> UpdateProductAsync(string id, Product updatedProduct)
        {
            var product = await _dbContext.Products.Find(p => p.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();

            if (product == null)
                return null;

            // Update product fields
            product.Name = updatedProduct.Name ?? product.Name;
            product.Color = updatedProduct.Color ?? product.Color;
            product.Qty = updatedProduct.Qty != 0 ? updatedProduct.Qty : product.Qty;
            product.Size = updatedProduct.Size ?? product.Size;

            await _dbContext.Products.ReplaceOneAsync(p => p.Id == ObjectId.Parse(id), product);

            return product;
        }

        // Delete a product
        public async Task<bool> DeleteProductAsync(string id)
        {
            var result = await _dbContext.Products.DeleteOneAsync(p => p.Id == ObjectId.Parse(id));
            return result.DeletedCount > 0;
        }

        // Get all products (Optional)
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _dbContext.Products.Find(_ => true).ToListAsync();
        }

      
    }
}
