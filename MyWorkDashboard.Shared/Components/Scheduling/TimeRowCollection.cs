﻿namespace MyWorkDashboard.Shared;

public class TimeRowCollection
{
    public TimeRow[] Rows { get; }
    public static int RowHeight = 60;
    int _pixelByMinute => RowHeight / 60;

    public event EventHandler<TimeRowSelectEventArgs> RowSelected;

    public TimeRowCollection()
    {
        Rows = Enumerable.Range(0, 24)
            .Select(i => new TimeRow(new TimeOnly(i, 0), i * RowHeight))
            .ToArray();

        foreach (TimeRow row in Rows)
        {
            row.Selected += RowOnSelected;
        }

    }

    private void RowOnSelected(object? sender, TimeRowSelectEventArgs e)
    {
        RowSelected?.Invoke(this, e);
    }

    public int TimeToPixel(TimeOnly time)
    {
        var nearyTime = Rows.First(r => r.Time.Hour == time.Hour);
        int offset = time.Minute * _pixelByMinute;
        return nearyTime.RowTopPosition + offset;
    }

    public TimeOnly PixelToTime(int pixcel, int accuracy = 10)
    {
        TimeOnly time = new TimeOnly(0, 0, 0);
        var minute = pixcel / _pixelByMinute;
        // 切り捨てる
        var syo = minute / accuracy;
        minute = syo * accuracy;
        time = time.AddMinutes(minute);
        return time;
    }
}