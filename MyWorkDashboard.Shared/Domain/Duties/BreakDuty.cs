namespace MyWorkDashboard.Shared.Duties;

public class BreakDuty : Duty
{
    public BreakDuty(string id, DateOnly date, WorkTimeRange timeRange, WorkTask workTask) 
        : base(id, date, timeRange, workTask)
    {
    }

    public override Duty Duplicate(string newId)
    {
        return new BreakDuty(newId, Date, _timeRange.DeepCopy(), _workTask.DeepCopy());
    }
}