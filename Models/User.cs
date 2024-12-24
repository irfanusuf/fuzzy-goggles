using System;
using MongoDB.Bson;

namespace WebApplication1.Models;

public class User
{

        public ObjectId Id { get; set; } // MongoDB generates ObjectId automatically  // not for dotnet 
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

}


  