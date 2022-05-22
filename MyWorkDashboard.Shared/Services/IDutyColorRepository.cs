namespace MyWorkDashboard.Shared.Services;

public interface IDutyColorRepository
{
    string GetHtmlColorCodeById(string workCodeFamilyId);
}