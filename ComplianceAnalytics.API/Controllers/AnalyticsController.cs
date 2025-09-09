using ComplianceAnalytics.Infrastructure.DTO;
using ComplianceAnalytics.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAnalytics.API.Controllers;

[ApiController]
[Route("api/reports")]
public class AnalyticsController : ControllerBase
{
    private readonly AnalyticsService _service;

    public AnalyticsController(AnalyticsService service)
    {
        _service = service;
    }

    [HttpGet("compliance")]
    public async Task<IActionResult> GetCompliance([FromQuery] AnalyticsFilterUser filter)
    {
        // For now, we can hardcode executedBy as "APIUser" or extract from JWT
        string executedBy = User?.Identity?.Name ?? "APIUser";

        var result = await _service.GetComplianceAnalyticsAsync(filter, executedBy);
        return Ok(result);
    }

    // Managers can get analytics for their own region
    [HttpGet("compliance/region")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> GetRegionCompliance([FromQuery] AnalyticsFilterManager filter)
    {
        // Ensure region is set from user's claims
        filter.Region = User.FindFirst("Region")?.Value;
        var result = await _service.GetComplianceAnalyticsAsync(filter, User.Identity.Name);
        return Ok(result);
    }
}
