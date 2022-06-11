namespace MyWorkDashboard.Shared;

public interface IDutyColorRepository
{
    string GetHtmlColorCodeById(string workCodeFamilyId);

    void Register(string id, string colorCode);
}