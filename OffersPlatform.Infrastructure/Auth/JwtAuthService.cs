using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Mapping;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Infrastructure.Auth;

public class JwtAuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public JwtAuthService(IConfiguration configuration, IPasswordHasher passwordHasher,
        IUserRepository userRepository, ICompanyRepository companyRepository, IMapper mapper)
    {
        _configuration = configuration;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<string> RegisterUserAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        
        return GenerateJwtToken(user.Role, user.Id);
    }

    public async Task<string> RegisterCompanyAsync(Company company, string password, CancellationToken cancellationToken = default)
    {
        return GenerateJwtToken(company.Role, company.Id);
    }
    

    public async Task<string> LoginAsync(string? email, string? password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Email and password must be provided.");

        // Try finding a user
        var userDto = await _userRepository.GetActiveUserByEmailAsync(email, cancellationToken);
        if (userDto != null)
        {
            var user = _mapper.Map<User>(userDto);
            return GenerateJwtToken(user.Role, user.Id);
        }

        // Try finding a company
        var companyDto = await _companyRepository.GetCompanyByEmailAsync(email, cancellationToken);
        if (companyDto != null)
        {
            var company = _mapper.Map<CompanyDto>(companyDto);
            return GenerateJwtToken(company.Role, company.Id);
        }

        // If neither matched
        throw new UnauthorizedAccessException("Invalid email or password.");
    }


    private string GenerateJwtToken(UserRole role, Guid id)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? throw new ArgumentNullException("JWT Secret is missing"));

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
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}