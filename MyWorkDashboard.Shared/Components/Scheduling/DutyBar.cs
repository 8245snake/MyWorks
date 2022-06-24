using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MyWorkDashboard.Shared.Duties;
using MyWorkDashboard.Shared.Services;

namespace MyWorkDashboard.Shared;

public class DutyBar
{
    /// <summary>
    /// モデル
    /// </summary>
    public Duty Duty { get; }
    /// <summary>
    /// Top位置
    /// </summary>
    public double Top { get; set; }
    /// <summary>
    /// 高さ
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// 選択中か否か
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// 多重度
    /// </summary>
    public int Multiplicity { get; set; } = 1;

    /// <summary>
    /// インデックス
    /// </summary>
    public int Index { get; set; } = 0;

    public TimeRowCollection? TimeRows { get; }

    private string _colorCode;

    public static int TimeWidth = 40;

    public event EventHandler<DutyBarEventArgs> Selected;
    public event EventHandler<DutyBarEventArgs> Dropped;
    public event EventHandler RedrawInvoked;

    private readonly bool _isAbsolutePositioning;

    /// <summary>
    /// 絶対位置を指定して初期化します
    /// </summary>
    /// <param name="duty"></param>
    /// <param name="top"></param>
    /// <param name="height"></param>
    /// <param name="colorCode"></param>
    public DutyBar(Duty duty, int top, int height, string colorCode, TimeRowCollection timeRows)
    {
        Duty = duty;
        Top = top;
        Height = height;
        IsSelected = false;
        _colorCode = colorCode;
        TimeRows = timeRows;
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
        TimeRows = null;
        _isAbsolutePositioning = false;
    }

     public string ColorCode => _colorCode;

    public string BarStyle
    {
        get
        {
            if (_isAbsolutePositioning)
            {
                string widthStyle = $"calc(calc(95% - {TimeWidth}px) / {Multiplicity})";
                string styel = "";
                //styel += $"top:{Top}px;";
                styel += $"width:{widthStyle};";
                styel += $"left:calc({TimeWidth}px + calc({Index} * {widthStyle}));";
                styel += $"height:{Height}px;";
                styel += $"background-color: {_colorCode};";
                return styel;
            }

            return $"height:{Height}px; background-color: {_colorCode};";
        }
    }

    public string AdditionalClass => IsSelected ? "bar-selected bar-selected-color" : "";

    public void OnClick(MouseEventArgs e)
    {
        IsSelected = true;
        Selected?.Invoke(this, new DutyBarEventArgs(this));
    }

    public void OnDrop(MouseEventArgs e)
    {
        Dropped?.Invoke(this, new DutyBarEventArgs(this));
    }

    /// <summary>
    /// 指定したタスクと時刻が重なっているか判定する
    /// </summary>
    /// <param name="other">比較相手</param>
    /// <returns>true:重なっている<br></br>false:重なっていない</returns>
    public bool IsOverlapping(DutyBar other)
    {
        if (this.Duty.StartTime >= other.Duty.StartTime && this.Duty.StartTime < other.Duty.EndTime)
        {
            // 自身の開始より前に始まって、自身の開始より後に終わっているタスク
            return true;
        }

        if (this.Duty.StartTime <= other.Duty.StartTime && this.Duty.EndTime > other.Duty.StartTime)
        {
            // 自身の開始より後かつ自身の終了より前に始まっているタスク
            return true;
        }

        return false;
    }

    /// <summary>
    /// UIの再描画を促す
    /// </summary>
    public void Redraw()
    {
        RedrawInvoked?.Invoke(this, EventArgs.Empty);
    }

}

