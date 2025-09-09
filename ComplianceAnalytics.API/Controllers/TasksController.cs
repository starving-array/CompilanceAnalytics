using ComplianceAnalytics.Application.DTO;
using ComplianceAnalytics.Application.Services;
using ComplianceAnalytics.Domain.Entities;
using ComplianceAnalytics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAnalytics.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskControllerService _taskControllerService;

        public TasksController(ITaskRepository taskRepository, ITaskControllerService taskControllerService)
        {
            _taskRepository = taskRepository;
            _taskControllerService = taskControllerService;
        }

        [HttpGet]
        public async Task<IEnumerable<TaskDTO>> GetAll()
        {
            return await _taskControllerService.GetAllTasks();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskEntity task)
        {
            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = task.TaskID }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskEntity updatedTask)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return NotFound();

            task.TaskName = updatedTask.TaskName;
            task.CompletionDate = updatedTask.CompletionDate;
            task.IsCompliant = updatedTask.IsCompliant;

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return NotFound();

            _taskRepository.Remove(task);
            await _taskRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
