using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDashboard.Shared.Mock;

/// <summary>
/// ローカルテスト用リポジトリ
/// </summary>
public class MockWorkCodeFamilyRepository : IWorkCodeFamilyRepository
{
    private List<WorkCodeFamily> _codeFamilies;
    private int _maxId = 0;

    public MockWorkCodeFamilyRepository()
    {
        _codeFamilies = new List<WorkCodeFamily>();
        Register(new WorkCodeFamily("F1", new WorkCategory("A", "開発"), new WorkCode("a1", "実装")));
        Register(new WorkCodeFamily("F2", new WorkCategory("A", "開発"), new WorkCode("a2", "レビュー")));
        Register(new WorkCodeFamily("F3", new WorkCategory("A", "開発"), new WorkCode("a3", "テスト")));
        Register(new WorkCodeFamily("F4", new WorkCategory("B", "保守"), new WorkCode("b1", "調査")));
        Register(new WorkCodeFamily("F5", new WorkCategory("B", "保守"), new WorkCode("b2", "現地対応")));
        _maxId = 5;
    }

    public string GetNewId()
    {
        _maxId++;
        return $"F{_maxId}";
    }

    public void Register(WorkCodeFamily workCodeFamily)
    {
        this.Delete(workCodeFamily.Id);
        _codeFamilies.Add(workCodeFamily);
    }

    public void Delete(string familyId)
    {
        var found = _codeFamilies.FirstOrDefault(f => f.Id == familyId);
        if (found != null)
        {
            _codeFamilies.Remove(found);
        }
    }

    public WorkCodeFamily? FindById(string familyId)
    {
        return _codeFamilies.FirstOrDefault(f => f.Id == familyId);
    }

    public WorkCodeFamily[] GetAll()
    {
        return _codeFamilies.ToArray();
    }
}