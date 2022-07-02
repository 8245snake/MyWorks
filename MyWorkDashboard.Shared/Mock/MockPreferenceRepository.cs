using MyWorkDashboard.Shared.UserPreferences;

namespace MyWorkDashboard.Shared.Mock;

public class MockPreferenceRepository : IPreferenceRepository
{
    private readonly List<DutyTemplate> _templates;
    private int _maxId;
    public MockPreferenceRepository()
    {
        _templates = new List<DutyTemplate>(new DutyTemplate[]
        {
            new DutyTemplate("1", "メールチェック"){IconName = "mail", Title = "メールチェック", WorkCodeFamilyId = "F1", Minute = 60},
            new DutyTemplate("2", "掃除"){IconName = "clear", Title = "机の掃除", WorkCodeFamilyId = "F2", Minute = 30},
            new DutyTemplate("3", "休憩"){IconName = "coffee", Title = "コーヒータイム", WorkCodeFamilyId = "F3", Minute = 10},
        });
        _maxId = 3;
    }

    private ThemePreference GetThemePreference()
    {
        return new ThemePreference() { IsDarkTheme = true };
    }

    public void SaveThemePreference(ThemePreference preference)
    {
    }

    public Task<ThemePreference> GetThemePreferenceAsync()
    {
        return Task.FromResult(GetThemePreference());
    }

    public Task SaveThemePreferenceAsync(ThemePreference preference)
    {
        SaveThemePreference(preference);
        return Task.CompletedTask;
    }

    public Task<DutyTemplate[]> GetAllDutyTemplatesAsync()
    {
        return Task.FromResult(_templates.ToArray());
    }

    public Task<string> GetNewId()
    {
        _maxId++;
        return Task.FromResult(_maxId.ToString());
    }

    public Task Update(DutyTemplate template)
    {
        Delete(template.Id);
        _templates.Add(template);
        return Task.CompletedTask;
    }

    public Task SaveAllTemplates(DutyTemplate[] templates)
    {
        _templates.Clear();
        _templates.AddRange(templates);
        return Task.CompletedTask;
    }

    public Task Delete(string templateId)
    {
        var found = _templates.FirstOrDefault(t => t.Id == templateId);
        if (found == null) return Task.CompletedTask;
        _templates.Remove(found);
        return Task.CompletedTask;
    }
}