using MongoDB.Bson;

namespace WebApplication1.Models
{
    public class User
    {
        public ObjectId Id { get; set; } 
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
    }
}
