using System.Runtime.InteropServices;

namespace MyWorkDashboard.Shared.Services;

public class ControlService
{
    private IFocusManeger _focusManeger;

    public ControlService(IFocusManeger focusManeger)
    {
        _focusManeger = focusManeger;
    }

    public void EnableImeMode()
    {
        _focusManeger?.EnableImeMode();
    }
}