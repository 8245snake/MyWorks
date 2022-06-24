using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MyWorkDashboard.Shared;

public class Utility
{
    [Conditional("DEBUG")]
    public static void Log<T>(T message, [CallerMemberName] string name = "")
    {
        Console.WriteLine($"{name} : {message}");
    }
}