using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Login
{
      [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }

}
