namespace MyWorkDashboard.Shared;

public class DutyBarCollection
{
    public List<DutyBar> Bars { get; }
    public event EventHandler<DutySelectedEventArgs> ItemSelected;

    public DutyBarCollection()
    {
        Bars = new List<DutyBar>();
    }

    public void Add(DutyBar bar)
    {
        Bars.Add(bar);
        bar.Selected -= BarOnSelected;
        bar.Selected += BarOnSelected;
    }

    public void Clear()
    {
        foreach (DutyBar bar in Bars)
        {
            bar.Selected -= BarOnSelected;
        }
        Bars.Clear();
    }

    private void BarOnSelected(object? sender, EventArgs e)
    {
        foreach (DutyBar bar in Bars)
        {
            if (bar != sender)
            {
                bar.IsSelected = false;
            }
        }

        var dutyBar = sender as DutyBar;
        if (dutyBar != null)
        {
            ItemSelected?.Invoke(this, new DutySelectedEventArgs(dutyBar.Duty));
        }
    }
}