namespace WorkBord.Duties;

public class WorkTimeRange
{
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }

    public WorkTimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    public WorkTimeRange(TimeOnly startTime, int spanMinutes)
    {
        StartTime = startTime;
        EndTime = startTime.AddMinutes(spanMinutes);
    }

    public override string ToString()
    {
        return $"{StartTime}~{EndTime}";
    }
}