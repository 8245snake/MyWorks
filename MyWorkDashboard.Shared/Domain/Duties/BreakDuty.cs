namespace MyWorkDashboard.Shared.Duties;

public class BreakDuty : Duty
{
    public BreakDuty(string id, DateOnly date, WorkTimeRange timeRange, WorkTask workTask) 
        : base(id, date, timeRange, workTask)
    {
    }
}