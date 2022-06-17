namespace MyWorkDashboard.Shared;

public interface IDutyColorRepository
{
    string GetHtmlColorCodeById(string workCodeFamilyId);

    void Register(string id, string colorCode);

    Task<string> GetHtmlColorCodeByIdAsync(string workCodeFamilyId);

    Task RegisterAsync(string id, string colorCode);
}