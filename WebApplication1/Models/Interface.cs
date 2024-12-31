public interface ITokenService
{
    string CreateToken(string userId, string email, string username);
}
