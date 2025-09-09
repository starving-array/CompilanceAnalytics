using ComplianceAnalytics.Application.DTO;
namespace ComplianceAnalytics.Application.Services
{
    public interface ITaskControllerService
    {
        // Service methods for task management
        public Task<IEnumerable<TaskDTO>> GetAllTasks();
    }
}