namespace ComplianceAnalytics.Infrastructure.DTO;
public class AnalyticsResult
{
    public SummaryKpi Summary { get; set; }
    public List<LocationCompliance> TopLocations { get; set; }
    public List<TrendData> ComplianceTrend { get; set; }
    public List<UserCompliance> TopUsers { get; set; }

}