using MyWorkDashboard.Shared.Mock;

namespace MyWorkDashboard.Shared.Services;

public class UserPreferenceService
{

    private readonly IPreferenceRepository _preferenceRepository;

    private bool _defaultDarkMode;

    public bool DefaultDarkMode
    {
        get => _defaultDarkMode;
        set
        {
            _defaultDarkMode = value;
            SaveLightMode(value);
        }
    }

    public UserPreferenceService()
    {
        _preferenceRepository = new MockPreferenceRepository();
        _defaultDarkMode = _preferenceRepository.GetThemePreferenceAsync().Result.IsDarkTheme;
    }

    public UserPreferenceService(IPreferenceRepository preferenceRepository)
    {
        _preferenceRepository = preferenceRepository;
        _defaultDarkMode = _preferenceRepository.GetThemePreferenceAsync().Result.IsDarkTheme;
    }

    private async void SaveLightMode(bool isDark)
    {
        var preference = await _preferenceRepository.GetThemePreferenceAsync();
        preference.IsDarkTheme = isDark;
        await _preferenceRepository.SaveThemePreferenceAsync(preference);
    }
}