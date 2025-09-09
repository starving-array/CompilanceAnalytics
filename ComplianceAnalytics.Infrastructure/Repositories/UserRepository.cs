using ComplianceAnalytics.Domain.Entities;
using ComplianceAnalytics.Domain.Repositories;
using ComplianceAnalytics.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComplianceAnalytics.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<UserEntity>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        
        public async Task<UserEntity?> GetByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
