using System;

namespace WebApplication1.Interfaces;

public interface ITokenService
{
 string CreateToken(string userId, string email, string username);
}
