using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDashboard.Shared.Service;

public class WorkCodeService
{
    private readonly IWorkCodeFamilyRepository _workCodeFamilyRepository;
    private readonly IDutyColorRepository _dutyColorRepository;

    public WorkCodeService(IWorkCodeFamilyRepository workCodeFamilyRepository, IDutyColorRepository dutyColorRepository)
    {
        _workCodeFamilyRepository = workCodeFamilyRepository;
        _dutyColorRepository = dutyColorRepository;
    }


    public string GetDefaultWorkCodeId()
    {
        return _workCodeFamilyRepository.GetAll().First().Id;
    }

    public Task<WorkCodeFamily[]> GetAllWorkCodeFamily()
    {
        return _workCodeFamilyRepository.GetAllAsync();
    }

    public Task<WorkCodeFamily?> FindWorkCodeFamilyById(string id)
    {
        return _workCodeFamilyRepository.FindByIdAsync(id);
    }

    public async Task SaveAll(IEnumerable<WorkCodeFamily> items)
    {
        await _workCodeFamilyRepository.RegisterAllAsync(items);
    }

    public Task RegisterColorAsync(string id, string colorCode)
    {
        return _dutyColorRepository.RegisterAsync(id, colorCode);
    }

    public Task<string> GetNewWorkCodeIdAsync()
    {
        return _workCodeFamilyRepository.GetNewIdAsync();
    }

    public async Task<string> GetDutyColorCodeAsync(string workCodeFamilyId)
    {
        if (string.IsNullOrWhiteSpace(workCodeFamilyId))
        {
            return "#cccccc";
        }
        return await _dutyColorRepository.GetHtmlColorCodeByIdAsync(workCodeFamilyId);
    }
}