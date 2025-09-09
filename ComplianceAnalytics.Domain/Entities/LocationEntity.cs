using System.ComponentModel.DataAnnotations;

namespace ComplianceAnalytics.Domain.Entities
{
    public class LocationEntity
    {
        [Key]
        public int LocationID { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

        public ICollection<TaskEntity> Tasks { get; set; }
    }
}
