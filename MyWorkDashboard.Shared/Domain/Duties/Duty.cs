namespace MyWorkDashboard.Shared.Duties;

public abstract class Duty
{
    public string Id { get; }
    public DateOnly Date { get; }

    protected WorkTimeRange _timeRange;
    protected WorkTask _workTask;

    public TimeOnly StartTime
    {
        get => _timeRange.StartTime;
        set => _timeRange.StartTime = value;
    }

    public TimeOnly EndTime
    {
        get => _timeRange.EndTime;
        set => _timeRange.EndTime = value;
    }

    public string Title
    {
        get => _workTask.Title;
        set => _workTask.Title = value;
    }

    public string Description
    {
        get => _workTask.Description;
        set => _workTask.Description = value;
    }

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

    public bool Contains(TimeOnly time)
    {
        return _timeRange.Contains(time);
    }


    public void Shift(TimeOnly start)
    {
        var minutes = (EndTime - StartTime).TotalMinutes;
        StartTime = start;
        EndTime = StartTime.AddMinutes(minutes);
    }
}