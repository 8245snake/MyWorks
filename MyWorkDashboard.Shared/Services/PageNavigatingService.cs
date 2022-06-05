namespace MyWorkDashboard.Shared.Services;

public class PageNavigatingService
{
    public event EventHandler Navigated;
    public PageType CurrentPage { get; private set; } = PageType.Home;

    public void Navigate(PageType page, object? sender)
    {
        CurrentPage = page;
        Navigated?.Invoke(sender, EventArgs.Empty);
    }



}


public enum PageType
{
    Home,
    Preference,
    Statistic,
}