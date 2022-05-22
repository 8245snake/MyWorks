namespace MyWorkDashboard.Shared.Components;

public class TimeRangeChangeEventArgs : EventArgs
{
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }

    public TimeRangeChangeEventArgs(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }
}