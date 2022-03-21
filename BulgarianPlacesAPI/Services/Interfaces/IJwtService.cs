using System.IdentityModel.Tokens.Jwt;

namespace BulgarianPlacesAPI.Services.Interfaces
{
    public interface IJwtService
    {
        string Generate(string id, string email);

        JwtSecurityToken Verify(string jwt);
    }
}
