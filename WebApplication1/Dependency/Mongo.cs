
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Dependency;

public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var databaseName = configuration["MongoDB:DatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

             public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

             public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
   
    }

