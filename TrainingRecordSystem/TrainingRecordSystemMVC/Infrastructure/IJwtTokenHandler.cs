using System.IdentityModel.Tokens.Jwt;

namespace TrainingRecordSystemMVC.Infrastructure
{
    public interface IJwtTokenHandler
    {
        JwtSecurityToken ReadJwtToken(string token);

    }
}
