using MyWorkDashboard.Shared.UserPreferences;
using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDashboard.Shared;

public class TemplateDutyVM
{
    DutyTemplate _dutyTemplate;
    private WorkCodeFamily? _workCode;
    public event EventHandler? PropertyChanged;

    public string Id => _dutyTemplate.Id;

    /// <summary>メニュー表示名</summary>
    public string MenuName
    {
        get { return _dutyTemplate.MenuName; }
        set
        {
            _dutyTemplate.MenuName = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>アイコン</summary>
    public string? IconName
    {
        get { return _dutyTemplate.IconName; }
        set
        {
            _dutyTemplate.IconName = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>タイトル</summary>
    public string Title
    {
        get { return _dutyTemplate.Title; }
        set
        {
            _dutyTemplate.Title = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>説明</summary>
    public string? Description
    {
        get { return _dutyTemplate.Description; }
        set
        {
            _dutyTemplate.Description = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>所要時間(分)</summary>
    public int Minute
    {
        get { return _dutyTemplate.Minute; }
        set
        {
            _dutyTemplate.Minute = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public WorkCodeFamily? WorkCode
    {
        get => _workCode;
        set
        {
            _workCode = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public TemplateDutyVM(DutyTemplate dutyTemplate)
    {
        _dutyTemplate = dutyTemplate;
    }

    public DutyTemplate GetDutyTemplate()
    {
        var clone = _dutyTemplate.DeepCopy();
        clone.WorkCodeFamilyId = WorkCode?.Id;
        return clone;
    }
}