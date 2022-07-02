using System.Collections;
using System.Reflection.Metadata.Ecma335;
using MyWorkDashboard.Shared.Duties;
using MyWorkDashboard.Shared.Mock;
using MyWorkDashboard.Shared.UserPreferences;

namespace MyWorkDashboard.Shared.Services;

public class UserPreferenceService
{
    public event EventHandler? TemplateUpdated;

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

    public Task<DutyTemplate[]> GetAllTemplates()
    {
        return _preferenceRepository.GetAllDutyTemplatesAsync();
    }

    public async Task<DutyTemplate> CreateNewTemplate()
    {
        var id = await _preferenceRepository.GetNewId();
        return new DutyTemplate(id, "定型タスク")
        {
            IconName = "mail",
            Minute = 60,
        };
    }

    public async Task SaveAllTemplates(DutyTemplate[] templates)
    {
        await _preferenceRepository.SaveAllTemplates(templates);
        TemplateUpdated?.Invoke(this, EventArgs.Empty);
    }

    public async Task UpdateTemplate(DutyTemplate template)
    {
        await _preferenceRepository.Update(template);
        TemplateUpdated?.Invoke(this, EventArgs.Empty);
    }
}