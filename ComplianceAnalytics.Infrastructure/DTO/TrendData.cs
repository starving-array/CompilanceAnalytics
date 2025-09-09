namespace ComplianceAnalytics.Infrastructure.DTO;
public class TrendData
{
    public DateTime TaskDate { get; set; }
    public int Total { get; set; }
    public int Compliant { get; set; }
    public decimal ComplianceRate { get; set; }
}