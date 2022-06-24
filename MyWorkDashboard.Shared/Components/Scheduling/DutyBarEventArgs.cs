using MyWorkDashboard.Shared.Duties;

namespace MyWorkDashboard.Shared;

public class DutyBarEventArgs : EventArgs
{
    public DutyBar DutyBar { get; }

    public DutyBarEventArgs(DutyBar dutyBar)
    {
        DutyBar = dutyBar;
    }
}