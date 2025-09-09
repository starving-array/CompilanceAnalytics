namespace ComplianceAnalytics.Infrastructure.DTO;
public class UserCompliance
{
    public string UserName { get; set; }
    public int CompletedTasks { get; set; }
    public decimal ComplianceRate { get; set; }
}