namespace WorkBord.Duties;

public abstract class Duty
{
    public string Id { get; }
    public DateOnly Date { get; }

    protected WorkTimeRange _timeRange;
    protected WorkTask _workTask;

    public TimeOnly StartTime => _timeRange.StartTime;
    public TimeOnly EndTime => _timeRange.EndTime;
    public string Title => _workTask.Title;
    public string Description => _workTask.Description;

    protected Duty(string id, DateOnly date, WorkTimeRange timeRange, WorkTask workTask)
    {
        Id = id;
        Date = date;
        _timeRange = timeRange;
        _workTask = workTask;
    }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Date)}: {Date}, {nameof(_timeRange)}: {_timeRange}, {nameof(_workTask)}: {_workTask}";
    }
}