using ComplianceAnalytics.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ComplianceAnalytics.Application.Services
{
    public class TaskService
    {
        private readonly List<TaskEntity> _tasks;

        public TaskService()
        {
            _tasks = new List<TaskEntity>();
        }

        public void AddTask(TaskEntity task)
        {
            _tasks.Add(task);
        }

        public IEnumerable<TaskEntity> GetAllTasks()
        {
            return _tasks;
        }

        public TaskEntity GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.TaskID == id);
        }

        public bool DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.TaskID != id);
            if (task != null)
            {
                _tasks.Remove(task);
                return true;
            }
            return false;
        }

    }
}