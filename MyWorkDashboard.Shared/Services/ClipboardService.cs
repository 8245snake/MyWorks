using MyWorkDashboard.Shared.Duties;

namespace MyWorkDashboard.Shared.Services;

public class ClipboardService
{
    private Duty? _duty;

    public Duty? Duty => _duty;

    public event EventHandler ClipboardChanged;

    public void Copy(Duty duty)
    {
        _duty = duty;
        ClipboardChanged?.Invoke(this, EventArgs.Empty);
    }

}