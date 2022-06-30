using MyWorkDashboard.Shared.UserPreferences;

namespace MyWorkDashboard.Shared.Mock;

public class MockPreferenceRepository : IPreferenceRepository
{
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
        return Task.FromResult(new DutyTemplate[]
        {
            new DutyTemplate("1", "なんか1"),
            new DutyTemplate("2", "なんか2"),
            new DutyTemplate("3", "なんか3"),
        });
    }
}