USE ComplianceAnalytics;
GO

CREATE OR ALTER PROCEDURE usp_GetComplianceAnalytics
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @LocationID INT = NULL,
    @Region NVARCHAR(100) = NULL,
    @WorkflowType NVARCHAR(100) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 30,
    @ExecutedBy NVARCHAR(200) = 'system'
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StartTime DATETIME = GETDATE();

    IF OBJECT_ID('tempdb..#FilteredTasks') IS NOT NULL DROP TABLE #FilteredTasks;

    CREATE TABLE #FilteredTasks (
        TaskID INT,
        TaskName NVARCHAR(200),
        LocationID INT,
        CompletedBy INT,
        CompletionDate DATETIME,
        IsCompliant BIT,
        WorkflowType NVARCHAR(50)
    );

    INSERT INTO #FilteredTasks (TaskID, TaskName, LocationID, CompletedBy, CompletionDate, IsCompliant, WorkflowType)
    SELECT t.TaskID, t.TaskName, t.LocationID, t.CompletedBy, t.CompletionDate, t.IsCompliant, t.WorkflowType
    FROM Tasks t
    INNER JOIN Locations l ON t.LocationID = l.LocationID
    WHERE (@StartDate IS NULL OR t.CompletionDate >= @StartDate)
      AND (@EndDate IS NULL OR t.CompletionDate <= @EndDate)
      AND (@LocationID IS NULL OR t.LocationID = @LocationID)
      AND (@Region IS NULL OR l.Region = @Region)
      AND (@WorkflowType IS NULL OR t.WorkflowType = @WorkflowType);

    -- Summary KPIs
    SELECT 
        COUNT(*) AS TotalTasks,
        SUM(CASE WHEN CompletionDate IS NOT NULL THEN 1 ELSE 0 END) AS CompletedTasks,
        CAST(SUM(CASE WHEN IsCompliant = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)) AS ComplianceRate,
        CAST(COUNT(*) * 1.0 / NULLIF(DATEDIFF(DAY, MIN(CompletionDate), MAX(CompletionDate)) + 1,0) AS DECIMAL(10,2)) AS AvgTasksPerDay
    FROM #FilteredTasks;

    -- Top 5 Locations
    SELECT TOP 5 
        l.LocationName,
        CAST(SUM(CASE WHEN t.IsCompliant = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)) AS ComplianceRate
    FROM #FilteredTasks t
    INNER JOIN Locations l ON t.LocationID = l.LocationID
    GROUP BY l.LocationName
    ORDER BY ComplianceRate ASC;

    -- Compliance Trend
    ;WITH Trend AS (
        SELECT 
            CAST(CompletionDate AS DATE) AS TaskDate,
            COUNT(*) AS Total,
            SUM(CASE WHEN IsCompliant = 1 THEN 1 ELSE 0 END) AS Compliant
        FROM #FilteredTasks
        GROUP BY CAST(CompletionDate AS DATE)
    )
    SELECT TaskDate, Total, Compliant, 
           CAST(Compliant * 100.0 / NULLIF(Total,0) AS DECIMAL(5,2)) AS ComplianceRate
    FROM Trend
    ORDER BY TaskDate
    OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;

    -- Top 5 Users
    SELECT TOP 5 
        u.UserName,
        COUNT(*) AS CompletedTasks,
        CAST(SUM(CASE WHEN t.IsCompliant = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*),0) AS DECIMAL(5,2)) AS ComplianceRate
    FROM #FilteredTasks t
    INNER JOIN Users u ON t.CompletedBy = u.UserID
    WHERE t.CompletionDate IS NOT NULL
    GROUP BY u.UserName
    ORDER BY CompletedTasks DESC;

    -- Log execution
    DECLARE @ExecutionTime INT = DATEDIFF(MILLISECOND, @StartTime, GETDATE());
    INSERT INTO ProcedureExecutionLog (ProcedureName, Params, ExecutionTime, RowsReturned, ExecutedBy)
    VALUES (
        'usp_GetComplianceAnalytics',
        CONCAT('StartDate=',COALESCE(CONVERT(VARCHAR,@StartDate),'NULL'),
               '; EndDate=',COALESCE(CONVERT(VARCHAR,@EndDate),'NULL'),
               '; LocationID=',COALESCE(CONVERT(VARCHAR,@LocationID),'NULL'),
               '; Region=',COALESCE(@Region,'NULL'),
               '; WorkflowType=',COALESCE(@WorkflowType,'NULL')),
        @ExecutionTime,
        @@ROWCOUNT,
        @ExecutedBy
    );

    DROP TABLE #FilteredTasks;
END;
