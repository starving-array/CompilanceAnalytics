using ComplianceAnalytics.Domain.Entities;

namespace ComplianceAnalytics.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<UserEntity>
    {
        Task<UserEntity?> GetByUserNameAsync(string username);
    }
}
