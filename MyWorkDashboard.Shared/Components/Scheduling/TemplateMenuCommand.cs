using System.Windows.Input;
using MyWorkDashboard.Shared.UserPreferences;

namespace MyWorkDashboard.Shared;

public class TemplateMenuCommand : ICommand
{
    public bool CanExecute(object? parameter) => true;
    public event EventHandler? CanExecuteChanged;
    public event EventHandler? Excuted;

    private readonly DutyTemplate _dutyTemplate;
    public TimeRow TimeRow { get; }

    public string IconName => _dutyTemplate.IconName;
    public string MenuName => _dutyTemplate.MenuName;
    public string TemplateId => _dutyTemplate.Id;

    public TemplateMenuCommand(DutyTemplate dutyTemplate, TimeRow timeRow)
    {
        _dutyTemplate = dutyTemplate;
        TimeRow = timeRow;
    }


    public void Execute(object? parameter)
    {
        Excuted?.Invoke(this, EventArgs.Empty);
    }
}