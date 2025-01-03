using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Interfaces;

public class TokenService : ITokenService   // inheritance 
{
    private readonly string _secretKey;     // 

    public TokenService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("SecretKey is not configured.");
    }

    public string CreateToken(string userId, string email, string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();   // intializing new instance of  JwtSecurityTokenHandler

        var key = Encoding.ASCII.GetBytes(_secretKey);  

        var tokenDescriptor = new SecurityTokenDescriptor      // sign contract 
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, username)
            ]),
            Expires = DateTime.UtcNow.AddHours(1),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);   
         
        return tokenHandler.WriteToken(token);
    }
}
