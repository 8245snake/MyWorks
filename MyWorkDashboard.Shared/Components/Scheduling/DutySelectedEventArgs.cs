using MyWorkDashboard.Shared.Duties;

namespace MyWorkDashboard.Shared;

public class DutySelectedEventArgs : EventArgs
{
    public Duty SelectedDuty { get; }

    public DutySelectedEventArgs(Duty selectedDuty)
    {
        SelectedDuty = selectedDuty;
    }
}