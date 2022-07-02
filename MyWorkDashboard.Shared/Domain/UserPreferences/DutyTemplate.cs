
namespace MyWorkDashboard.Shared.UserPreferences;

public class DutyTemplate
{
    /// <summary>識別子</summary>
    public string Id { get; }

    /// <summary>メニュー表示名</summary>
    public string MenuName { get; set; }

    /// <summary>アイコン</summary>
    public string IconName { get; set; }

    /// <summary>タイトル</summary>
    public string Title { get; set; }

    /// <summary>説明</summary>
    public string? Description { get; set; }

    /// <summary>作業コード</summary>
    public string? WorkCodeFamilyId { get; set; }

    /// <summary>並び順</summary>
    public int Order { get; set; }

    /// <summary>所要時間(分)</summary>
    public int Minute { get; set; }

    public DutyTemplate(string id, string menuName)
    {
        Id = id;
        MenuName = menuName;
        Order = 0;
        Minute = 60;
    }

    public override string ToString()
    {
        return $"{nameof(MenuName)}: {MenuName}";
    }

    public DutyTemplate DeepCopy()
    {
        return new DutyTemplate(this.Id, this.MenuName)
        {
            Description = this.Description,
            WorkCodeFamilyId = this.WorkCodeFamilyId,
            Order = this.Order,
            Minute = this.Minute,
            IconName = this.IconName,
            Title = this.Title,
        };
    }
}