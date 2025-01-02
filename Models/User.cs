
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        public ObjectId Id { get; set; }

        public required string Username { get; set; }

    
        [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }
        public string? Phone { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string? Role { get; set; }
        public string? ProfilePictureUrl { get; set; }




    }




}