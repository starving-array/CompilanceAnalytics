

using ComplianceAnalytics.Application.DTO;
using ComplianceAnalytics.Domain.Repositories;

namespace ComplianceAnalytics.Application.Services
{
    public class TaskControllerService:ITaskControllerService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskControllerService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }   
        public async Task<IEnumerable<TaskDTO>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Select(task => TaskDTO.FromEntity(task));
        }
    }
}
