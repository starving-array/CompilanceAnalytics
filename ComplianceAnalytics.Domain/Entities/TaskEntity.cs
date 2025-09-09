using System.ComponentModel.DataAnnotations;

namespace ComplianceAnalytics.Domain.Entities
{
    public class TaskEntity
    {

        [Key]
        public int TaskID { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public int LocationID { get; set; }
        public int CompletedBy { get; set; }   // UserID of person who completed it
        public DateTime CompletionDate { get; set; }
        public bool IsCompliant { get; set; }
        public string WorkflowType { get; set; } = string.Empty;

        public LocationEntity Location { get; set; }
        public UserEntity CompletedUser { get; set; }
    }
}
