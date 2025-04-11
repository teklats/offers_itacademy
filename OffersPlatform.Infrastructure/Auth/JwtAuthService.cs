using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Infrastructure.Auth;

public class JwtAuthService : IAuthService
{
    private readonly IConfiguration _configuration;


    public JwtAuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string RegisterUser(User user, string password)
    {

        return GenerateJwtToken(user.Role, user.Id);
    }

    public string RegisterCompany(Company company, string password)
    {
        return GenerateJwtToken(company.Role, company.Id);
    }


    public string Login(UserRole role, Guid id)
    {
        return GenerateJwtToken(role, id);
    }



    private string GenerateJwtToken(UserRole role, Guid id)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ??
                                         throw new Exception("JWT Secret is missing"));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, role.ToString()),
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
