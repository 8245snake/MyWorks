using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using MyWorkDashboard.Shared.Services;

namespace MyWorkDesktop.Services;

public class FocusManeger : IFocusManeger
{

    private IntPtr _hwnd;

    public FocusManeger(IntPtr handle)
    {
        _hwnd = handle;
    }

    public void EnableImeMode()
    {
        // todo IMEモードを変える方法を探す
    }
}