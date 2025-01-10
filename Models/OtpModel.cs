using System;
using MongoDB.Bson;

namespace WebApplication1.Models;

public class OtpModel
{

        public ObjectId Id { get; set; }

        public required string Otp { get; set; }


        
}
