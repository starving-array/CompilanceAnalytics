using Xunit;
using Moq;
using ComplianceAnalytics.Infrastructure.Service;
using ComplianceAnalytics.Domain.Repositories;
using ComplianceAnalytics.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ComplianceAnalytics.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly IConfiguration _config;
        private readonly AuthService _authService;


        public AuthServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "supersecretkey123456supersecretkey123456supersecretkey123456" },
                { "Jwt:Issuer", "ComplianceAnalytics" },
                { "Jwt:Audience", "ComplianceAnalyticsUsers" },
                { "Jwt:ExpireMinutes", "60" }
            };
            _config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings!).Build();
            _authService = new AuthService(_config,_userRepoMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsValid()
        {
            // Arrange
            var user = new UserEntity { UserName = "testuser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), Role = "Admin" };
            _userRepoMock.Setup(r => r.GetByUserNameAsync("testuser")).ReturnsAsync(user);

            // Act
            var token = await _authService.LoginAsync("testuser", "password");

            // Assert
            Assert.NotNull(token);
            Assert.Contains(".", token); // basic JWT structure check
        }

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenInvalidPassword()
        {
            // Arrange
            var user = new UserEntity { UserName = "testuser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            _userRepoMock.Setup(r => r.GetByUserNameAsync("testuser")).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync("testuser", "wrongpass"));
        }
    }
}
