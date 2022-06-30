
namespace MyWorkDashboard.Shared.UserPreferences;

public class DutyTemplate
{
    /// <summary>識別子</summary>
    public string Id { get; }

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

    public DutyTemplate(string id, string title)
    {
        Id = id;
        Title = title;
        Order = 0;
        Minute = 60;
    }

    public DutyTemplate(string id, string title, string description, string workCodeFamilyId, int order, int minute)
    {
        Id = id;
        Title = title;
        Description = description;
        WorkCodeFamilyId = workCodeFamilyId;
        Order = order;
        Minute = minute;
    }
}