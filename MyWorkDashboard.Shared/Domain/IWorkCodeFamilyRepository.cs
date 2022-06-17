using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDashboard.Shared;

/// <summary>
/// 作業コードのリポジトリ抽象
/// </summary>
public interface IWorkCodeFamilyRepository
{
    string GetNewId();
    void RegisterAll(IEnumerable<WorkCodeFamily> families);
    WorkCodeFamily? FindById(string familyId);
    WorkCodeFamily[] GetAll();

    Task<string> GetNewIdAsync();
    Task RegisterAllAsync(IEnumerable<WorkCodeFamily> families);
    Task<WorkCodeFamily?> FindByIdAsync(string familyId);
    Task<WorkCodeFamily[]> GetAllAsync();
}