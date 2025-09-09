using ComplianceAnalytics.Domain.Entities;
using ComplianceAnalytics.Infrastructure.DTO.Auth;

namespace ComplianceAnalytics.Infrastructure.Service
{
    public interface IAuthService
    {
        Task<UserEntity> RegisterAsync(string username, string password, string role, string region = null);
        Task<string> LoginAsync(string username, string password);
    }
}
