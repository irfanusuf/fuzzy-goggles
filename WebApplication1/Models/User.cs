using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        public ObjectId Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [JsonIgnore]
        public string Password { get; set; }

        public string? Phone { get; set; }
        public DateTime UserDateCreated { get; set; }
        public DateTime? UserDateModified { get; set; }
        public string? Role { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
