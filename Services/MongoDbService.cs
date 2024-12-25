using System;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class MongoDbService 
{

        private readonly IMongoDatabase _database;


            // function which takes connection string from config and establishes a connection with mongodb server 
        public MongoDbService (IConfiguration configuration){

            var connectionString = configuration["MongoDB:ConnectionString"];  // importing connection strting 
            var databaseName = configuration["MongoDB:DatabaseName"];     //importing data base name from appsettings

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            
            _database =  database;

        }

        public IMongoCollection<Product> Products => _database.GetCollection <Product>("Products");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");





}
