using System;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using ComplianceAnalytics.Infrastructure.DTO;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;
namespace ComplianceAnalytics.Infrastructure.Service;
using Serilog;

public class AnalyticsService
{
    private readonly IDistributedCache _cache;
    private readonly string _connectionString;

    public AnalyticsService(IDistributedCache cache, string connectionString)
    {
        _cache = cache;
        _connectionString = connectionString;
    }

    public async Task<AnalyticsResult> GetComplianceAnalyticsAsync(AnalyticsFilter filter, String executedBy)
    {
        string cacheKey = $"compliance_{filter.WorkflowType}_{filter.PageNumber}_{filter.PageSize}";

        // 1️⃣ Try cache first
        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            Log.Information("Cache hit for {CacheKey}", cacheKey);
            return JsonSerializer.Deserialize<AnalyticsResult>(cachedData);
        }

        Console.WriteLine("Cache miss");

        // 2️⃣ Query DB if not cached
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        var region = (string)null;
        if (filter is AnalyticsFilterManager analyticsFilterManager)
        {
            region = analyticsFilterManager.Region;

        }

        using var multi = await connection.QueryMultipleAsync(
            "usp_GetComplianceAnalytics",
            new
            {

                // You can now access properties specific to AnalyticsFilterUser
                Region = region,
                WorkflowType = filter.WorkflowType,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
            },
            commandType: CommandType.StoredProcedure
        );

        var start = DateTime.UtcNow;
        var result = new AnalyticsResult
        {
            Summary = await multi.ReadFirstOrDefaultAsync<SummaryKpi>(),
            TopLocations = (await multi.ReadAsync<LocationCompliance>()).AsList(),
            ComplianceTrend = (await multi.ReadAsync<TrendData>()).AsList(),
            TopUsers = (await multi.ReadAsync<UserCompliance>()).AsList()
        };
        var elapsed = DateTime.UtcNow - start;


        Log.Information("Executed usp_GetComplianceAnalytics (Region={Region}, WorkflowType={WorkflowType}) in {Elapsed}ms",
                region, filter.WorkflowType, elapsed.TotalMilliseconds);
        // 3️⃣ Store in Redis
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });

        return result;
    }
}
