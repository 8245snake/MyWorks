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

    /// <summary>
    /// 重なりを判定して多重度とインデックスを設定する
    /// </summary>
    public void CalculateMultiplicity()
    {
        foreach (DutyBar bar in Bars)
        {
            // １つずつ重なりを見ていく
            var overlapping = Bars.Where(b => b.IsOverlapping(bar)).OrderBy(b => b.Duty.StartTime).ToArray();
            bar.Multiplicity = overlapping.Length;
            bar.Index = Array.IndexOf(overlapping, bar);
        }

        // todo A-B B-C のように重なっている場合に多重度を3にしてインデックスを振る実装
    }
}