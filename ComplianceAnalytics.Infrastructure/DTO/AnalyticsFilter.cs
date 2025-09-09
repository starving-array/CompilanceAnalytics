namespace ComplianceAnalytics.Infrastructure.DTO;
public class AnalyticsFilter
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? LocationID { get; set; }
    public string Region { get; set; }
    public string WorkflowType { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 30;
    public string? ExecutedBy { get; internal set; }
}