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
}