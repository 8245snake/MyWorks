using Microsoft.AspNetCore.Components.Web;
using MyWorkDashboard.Shared.Duties;

namespace MyWorkDashboard.Shared;

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

    public string AdditionalClass => IsSelected ? "bar-selected bar-selected-color" : "";

    public Action<MouseEventArgs> OnClick => e =>
    {
        IsSelected = true;
        Selected?.Invoke(this, EventArgs.Empty);
    };
}