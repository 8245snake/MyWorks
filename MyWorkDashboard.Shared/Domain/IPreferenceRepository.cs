using MyWorkDashboard.Shared.UserPreferences;

namespace MyWorkDashboard.Shared;

public interface IPreferenceRepository
{
    Task<ThemePreference> GetThemePreferenceAsync();
    Task SaveThemePreferenceAsync(ThemePreference preference);
    Task<DutyTemplate[]> GetAllDutyTemplatesAsync();
    Task<String> GetNewId();
    Task Update(DutyTemplate template);
    Task SaveAllTemplates(DutyTemplate[] templates);
    Task Delete(string templateId);
}