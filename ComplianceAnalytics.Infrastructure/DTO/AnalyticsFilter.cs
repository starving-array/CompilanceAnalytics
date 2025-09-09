namespace ComplianceAnalytics.Infrastructure.DTO;
public class AnalyticsFilter
{
    public string WorkflowType { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 30;
}