namespace MyWorkDashboard.Shared;

public class TimeRowSelectEventArgs : EventArgs
{
    public TimeOnly Time { get; }
    public TimeRowSelectEventArgs(TimeOnly time)
    {
        Time = time;
    }
}