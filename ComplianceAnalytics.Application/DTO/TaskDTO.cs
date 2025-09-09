using System.ComponentModel.DataAnnotations;
using ComplianceAnalytics.Domain.Entities;

namespace ComplianceAnalytics.Application.DTO
{
    public class TaskDTO
    {
        [Key]
        public int TaskID { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public int LocationID { get; set; }
        public int CompletedBy { get; set; }   // UserID of person who completed it
        public DateTime CompletionDate { get; set; }
        public bool IsCompliant { get; set; }
        public string WorkflowType { get; set; } = string.Empty;

        public TaskEntity ToEntity()
        {
            return new TaskEntity
            {
                TaskID = this.TaskID,
                TaskName = this.TaskName,
                LocationID = this.LocationID,
                CompletedBy = this.CompletedBy,
                CompletionDate = this.CompletionDate,
                IsCompliant = this.IsCompliant,
                WorkflowType = this.WorkflowType
            };
        }

        public static TaskDTO FromEntity(TaskEntity entity)
        {
            return new TaskDTO
            {
                TaskID = entity.TaskID,
                TaskName = entity.TaskName,
                LocationID = entity.LocationID,
                CompletedBy = entity.CompletedBy,
                CompletionDate = entity.CompletionDate,
                IsCompliant = entity.IsCompliant,
                WorkflowType = entity.WorkflowType
            };
        }
    }
}
