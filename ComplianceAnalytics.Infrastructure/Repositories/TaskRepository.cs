using ComplianceAnalytics.Domain.Entities;
using ComplianceAnalytics.Domain.Repositories;
using ComplianceAnalytics.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComplianceAnalytics.Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<TaskEntity>> GetTasksByLocationAsync(int locationId)
        {
            return await _dbSet
                .Include(t => t.Location)
                .Include(t => t.CompletedUser)
                .Where(t => t.LocationID == locationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByUserAsync(int userId)
        {
            return await _dbSet
                .Include(t => t.Location)
                .Where(t => t.CompletedBy == userId)
                .ToListAsync();
        }
    }
}
