using ComplianceAnalytics.Domain.Entities;

namespace ComplianceAnalytics.Domain.Repositories
{
    public interface ITaskRepository : IGenericRepository<TaskEntity>
    {
        Task<IEnumerable<TaskEntity>> GetTasksByLocationAsync(int locationId);
        Task<IEnumerable<TaskEntity>> GetTasksByUserAsync(int userId);
    }
}
