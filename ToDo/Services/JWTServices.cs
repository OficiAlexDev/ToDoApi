using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDo.Models;

namespace ToDo.Services
{
    /// <summary>
    /// General Json Web Tokens services
    /// </summary>
    /// <param name="configuration">IConfiguration used to get JWT Secret</param>
    public class JWTServices(IConfiguration configuration)
    {
        /// <summary>
        /// Generating JWT based in user credentials
        /// </summary>
        /// <param name="user">User Model</param>
        /// <returns>JWT Token</returns>
        public string GenerateToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new();           
            SecurityTokenDescriptor securityTokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new(ClaimTypes.Name, user.Username),
                    new("Id", user.Id.ToString()),
                }),
                //The token will expire in 30mins because has 5mins of delay
                Expires = DateTime.UtcNow.AddMinutes(25),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(JWTSecret.Key(configuration)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            return tokenHandler.WriteToken(tokenHandler.CreateToken(securityTokenDescriptor));
        }
    }
}
