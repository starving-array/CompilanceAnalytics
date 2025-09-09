using System.ComponentModel.DataAnnotations;

namespace ComplianceAnalytics.Domain.Entities
{
    public class UserEntity
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;  // Admin, Manager, User

        public string PasswordHash { get; set; } = string.Empty; 

        public string? Region { get; set; }  // Optional region for Manager/User roles

        public ICollection<TaskEntity> TasksCompleted { get; set; }
    }
}
