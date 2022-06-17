using Microsoft.AspNetCore.Components.Web;

namespace MyWorkDashboard.Shared;

public class TimeRow
{
    public TimeOnly Time { get; }
    public int RowTopPosition { get; }

    public event EventHandler<TimeRowSelectEventArgs> Selected;

    public TimeRow(TimeOnly time, int rowTopPosition)
    {
        Time = time;
        RowTopPosition = rowTopPosition;
    }

    public Action<MouseEventArgs> OnClick => e =>
    {
        Selected?.Invoke(this, new TimeRowSelectEventArgs(Time));
    };
}