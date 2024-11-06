using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Core.DTOs;
using TaskManagement.Infrastructure.Interfaces;
using System.Security.Cryptography;

namespace TaskManagement.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IConfiguration _configuration;
        public AuthenticationService(IAuthenticationRepository authenticationRepository, IConfiguration configuration) 
        { 
            _authenticationRepository = authenticationRepository;
            _configuration = configuration;
        }

        

        public async Task<string?> Login(LoginCredentails loginDetails)
        {
            loginDetails.Password = HashPassword(loginDetails.Password);
            var existUser = await _authenticationRepository.Login(loginDetails);
            if (existUser == null)
            {
                return null; // return null to indicate login failure
            }
            return GenerateJwtToken(existUser);
        }

        private string GenerateJwtToken(TokenPayload user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);
            var secretKey = new SymmetricSecurityKey(key);
            var signCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("userName", user.Username),
                new Claim("userRole", user.Role.ToString()),
                new Claim("userId", user.Userid.ToString())
            };

            var tokenOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("PinCode cannot be null or empty.");

            using (var sha256 = SHA256.Create())
            {
                var hash = new StringBuilder();
                byte[] crypto = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                foreach (byte theByte in crypto)
                {
                    hash.Append(theByte.ToString("x2"));
                }
                return hash.ToString();
            }
        }
    }
}
