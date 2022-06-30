using MyWorkDashboard.Shared.Duties;
using MyWorkDashboard.Shared.UserPreferences;

namespace MyWorkDashboard.Shared.Service;

public class TemplateService
{
    private readonly IPreferenceRepository _preferenceRepository;
    private readonly DutyTemplate[] _templates;
    public TemplateService(IPreferenceRepository preferenceRepository)
    {
        _preferenceRepository = preferenceRepository;
        _templates = _preferenceRepository.GetAllDutyTemplatesAsync().Result;
    }

    /// <summary>
    /// テンプレートから業務データを作成します
    /// </summary>
    /// <param name="templateId">元となる<see cref="DutyTemplate.Id"/></param>
    /// <param name="startTime">開始日時</param>
    /// <param name="service">サービス</param>
    /// <returns>作成した業務データ</returns>
    public async Task<BusinessDuty> CreateNewDutyFromTemplate(string templateId, DateTime startTime, DutyService service)
    {
        var date = DateOnly.FromDateTime(startTime);
        var start = TimeOnly.FromDateTime(startTime);
        var end = start.AddMinutes(60);
        BusinessDuty newDuty = await service.CreateNewDutyAsync(date, new WorkTimeRange(start, end));
        DutyTemplate? template = _templates.FirstOrDefault(t => t.Id == templateId);
        if(template == null) return newDuty;

        newDuty.Title = template.Title;
        newDuty.Description = template.Description;
        newDuty.EndTime = newDuty.StartTime.AddMinutes(template.Minute);
        newDuty.WorkCodeFamilyId = template.WorkCodeFamilyId;
        return newDuty;
    }
}