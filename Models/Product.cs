using System;
using MongoDB.Bson;

namespace WebApplication1.Models;

  public class Product
    {
        public ObjectId Id { get; set; } // MongoDB generates ObjectId automatically  // not for dotnet 
        public required string Name { get; set; }
        public required string Size { get; set; }
        public int Quantity { get; set; }
    }
