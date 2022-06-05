using Microsoft.AspNetCore.Components.Web;
using MyWorkDashboard.Shared.Duties;

namespace MyWorkDashboard.Shared.Components;


public class TimeRowCollection
{
    public TimeRow[] Rows { get; }
    public static int RowHeight = 60;
    int _pixelByMinute => RowHeight / 60;

    public TimeRowCollection(Action<TimeOnly> appendingFunc)
    {
        Rows = Enumerable.Range(0, 24)
            .Select(i => new TimeRow(new TimeOnly(i, 0), i * RowHeight, appendingFunc))
            .ToArray();
    }

    public int TimeToPixel(TimeOnly time)
    {
        var nearyTime = Rows.First(r => r.Time.Hour == time.Hour);
        int offset = time.Minute * _pixelByMinute;
        return nearyTime.RowTopPosition + offset;
    }
}


public class TimeRow
{
    public TimeOnly Time { get; }
    public int RowTopPosition { get; }
    Action<TimeOnly> _appendingFunc;

    public TimeRow(TimeOnly time, int rowTopPosition, Action<TimeOnly> appendingFunc)
    {
        Time = time;
        RowTopPosition = rowTopPosition;
        _appendingFunc = appendingFunc;
    }

    public Action<MouseEventArgs> OnClick => e =>
    {
        _appendingFunc.Invoke(this.Time);
    };
}

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

public class DutySelectedEventArgs : EventArgs
{
    public Duty SelectedDuty { get; }

    public DutySelectedEventArgs(Duty selectedDuty)
    {
        SelectedDuty = selectedDuty;
    }
}

public class DutyBar
{
    public Duty Duty { get; }
    public int Top { get; }
    public int Height { get; }
    public bool IsSelected { get; set; }
    private string _colorCode;

    public static int TimeWidth = 40;

    public event EventHandler Selected;

    private readonly bool _isAbsolutePositioning;

    /// <summary>
    /// 絶対位置を指定して初期化します
    /// </summary>
    /// <param name="duty"></param>
    /// <param name="top"></param>
    /// <param name="height"></param>
    /// <param name="colorCode"></param>
    public DutyBar(Duty duty, int top, int height, string colorCode)
    {
        Duty = duty;
        Top = top;
        Height = height;
        IsSelected = false;
        _colorCode = colorCode;
        _isAbsolutePositioning = true;
    }

    /// <summary>
    /// モデルのみ指定して初期化します
    /// </summary>
    /// <param name="duty"></param>
    /// <param name="colorCode"></param>
    public DutyBar(Duty duty, int height, string colorCode)
    {
        Duty = duty;
        IsSelected = false;
        Height = height;
        _colorCode = colorCode;
        _isAbsolutePositioning = false;
    }

     public string ColorCode => _colorCode;

    public string BarStyle
    {
        get
        {
            if (_isAbsolutePositioning)
            {
                return $"top:{Top}px; height:{Height}px; left:{TimeWidth + 5}px; background-color: {_colorCode};";
            }

            return $"height:{Height}px; background-color: {_colorCode};";
        }
    }

    public string AdditionalClass => IsSelected ? "bar-selected" : "";

    public Action<MouseEventArgs> OnClick => e =>
    {
        IsSelected = true;
        Selected?.Invoke(this, EventArgs.Empty);
    };
}