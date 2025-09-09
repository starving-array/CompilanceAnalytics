namespace ComplianceAnalytics.Domain.Entities
{
    public class ProcedureExecutionLogEntity
    {
        public int ExecutionID { get; set; }
        public string ProcedureName { get; set; } = string.Empty;
        public string Params { get; set; } = string.Empty;
        public TimeSpan ExecutionTime { get; set; }
        public int RowsReturned { get; set; }
        public int ExecutedBy { get; set; }   // UserID
        public DateTime CreatedOn { get; set; }
    }
}
