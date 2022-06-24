namespace MyWorkDashboard.Shared;

public class DutyBarCollection
{
    public List<DutyBar> Bars { get; }
    public event EventHandler<DutyBarEventArgs> ItemSelected;
    public event EventHandler<DutyBarEventArgs> ItemDropped;

    public DutyBarCollection()
    {
        Bars = new List<DutyBar>();
    }

    public void Add(DutyBar bar)
    {
        Bars.Add(bar);
        bar.Selected -= BarOnSelected;
        bar.Selected += BarOnSelected;

        bar.Dropped -= OnDropped;
        bar.Dropped += OnDropped;
    }



    public void Clear()
    {
        foreach (DutyBar bar in Bars)
        {
            bar.Selected -= BarOnSelected;
            bar.Dropped -= OnDropped;
        }
        Bars.Clear();
    }

    private void BarOnSelected(object? sender, DutyBarEventArgs e)
    {
        foreach (DutyBar bar in Bars)
        {
            // 選択されたバー以外を非選択にする
            bar.IsSelected = (bar == sender);
        }

        if (sender is DutyBar dutyBar)
        {
            // イベント発火
            ItemSelected?.Invoke(this, e);
        }
    }

    private void OnDropped(object? sender, DutyBarEventArgs e)
    {
        ItemDropped?.Invoke(this, e);
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