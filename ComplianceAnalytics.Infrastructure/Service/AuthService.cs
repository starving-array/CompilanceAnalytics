using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ComplianceAnalytics.Infrastructure.DTO.Auth;
using ComplianceAnalytics.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using ComplianceAnalytics.Infrastructure.Repositories;
using ComplianceAnalytics.Infrastructure.Service;
using ComplianceAnalytics.Domain.Repositories; // from BCrypt.Net-Next NuGet package

namespace ComplianceAnalytics.Infrastructure.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Registers a new user with a hashed password.
        /// </summary>
        public async Task<UserEntity> RegisterAsync(string username, string password, string role, string region)
        {
            // hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new UserEntity
            {
                UserName = username,
                PasswordHash = passwordHash,
                Role = role,
                Region = region
            };
    
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUserNameAsync(username);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            // check password
            bool validPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!validPassword)
                throw new UnauthorizedAccessException("Invalid credentials");

            return GenerateJwtToken(user);
        }

        /// <summary>
        /// Generate JWT token with claims.
        /// </summary>
        private string GenerateJwtToken(UserEntity user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("role", user.Role),
                new Claim("userId", user.UserID.ToString())
            };

            if (!string.IsNullOrEmpty(user.Region))
            {
                claims.Add(new Claim("region", user.Region));
            }

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
