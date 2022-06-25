namespace MyWorkDashboard.Shared.Duties;

public class WorkTimeRange
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

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

    public TimeSpan Span => EndTime - StartTime;

    public bool Contains(TimeOnly time)
    {
        return StartTime <= time && time <= EndTime;
    }

    public WorkTimeRange DeepCopy()
    {
        return new WorkTimeRange(this.StartTime, this.EndTime);
    }

    public override string ToString()
    {
        return $"{StartTime}~{EndTime}";
    }
}