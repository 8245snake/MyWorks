using System.ComponentModel;
using WorkBord;
using WorkBord.WorkCodeFamilies;
using WorkBord.Duties;

namespace MyWorkDashboard.Shared.Services;

public class SchedulingServive : INotifyPropertyChanged
{

    public event EventHandler SelectedDutyChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public Duty SelectedDuty { get; private set; }

    public void ChangeSelectedDuty(Duty duty, object sender)
    {
        SelectedDuty = duty;
        SelectedDutyChanged.Invoke(sender, EventArgs.Empty);
    }
}