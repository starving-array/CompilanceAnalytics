namespace ComplianceAnalytics.Infrastructure.DTO;
public class SummaryKpi
{
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public decimal ComplianceRate { get; set; }
    public decimal AvgTasksPerDay { get; set; }
}
