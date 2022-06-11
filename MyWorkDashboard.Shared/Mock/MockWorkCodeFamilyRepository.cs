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
        Register(new WorkCodeFamily("F6", new WorkCategory("C", "打ち合わせ"), new WorkCode("d01", "打ち合わせ")));
        Register(new WorkCodeFamily("F7", new WorkCategory("D", "打ち合わせ"), new WorkCode("d02", "打ち合わせ")));
        Register(new WorkCodeFamily("F8", new WorkCategory("E", "打ち合わせ"), new WorkCode("d03", "打ち合わせ")));
        Register(new WorkCodeFamily("F9", new WorkCategory("F", "打ち合わせ"), new WorkCode("d04", "打ち合わせ")));
        Register(new WorkCodeFamily("F10", new WorkCategory("G", "打ち合わせ"), new WorkCode("d05", "打ち合わせ")));
        Register(new WorkCodeFamily("F11", new WorkCategory("H", "打ち合わせ"), new WorkCode("d06", "打ち合わせ")));
        Register(new WorkCodeFamily("F12", new WorkCategory("I", "打ち合わせ"), new WorkCode("d07", "打ち合わせ")));
        Register(new WorkCodeFamily("F13", new WorkCategory("C", "社内作業"), new WorkCode("d08", "社内作業")));
        Register(new WorkCodeFamily("F14", new WorkCategory("D", "社内作業"), new WorkCode("d09", "社内作業")));
        Register(new WorkCodeFamily("F15", new WorkCategory("E", "社内作業"), new WorkCode("d10", "社内作業")));
        Register(new WorkCodeFamily("F16", new WorkCategory("F", "社内作業"), new WorkCode("d11", "社内作業")));
        Register(new WorkCodeFamily("F17", new WorkCategory("G", "社内作業"), new WorkCode("d12", "社内作業")));
        Register(new WorkCodeFamily("F18", new WorkCategory("H", "社内作業"), new WorkCode("d13", "社内作業")));
        Register(new WorkCodeFamily("F19", new WorkCategory("I", "社内作業"), new WorkCode("d14", "社内作業")));
        Register(new WorkCodeFamily("F20", new WorkCategory("C", "その他"), new WorkCode("d15", "その他")));
        Register(new WorkCodeFamily("F21", new WorkCategory("D", "その他"), new WorkCode("d16", "その他")));
        Register(new WorkCodeFamily("F22", new WorkCategory("E", "その他"), new WorkCode("d17", "その他")));
        Register(new WorkCodeFamily("F23", new WorkCategory("F", "その他"), new WorkCode("d18", "その他")));
        Register(new WorkCodeFamily("F24", new WorkCategory("G", "その他"), new WorkCode("d19", "その他")));
        Register(new WorkCodeFamily("F25", new WorkCategory("H", "その他"), new WorkCode("d20", "その他")));
        Register(new WorkCodeFamily("F26", new WorkCategory("I", "その他"), new WorkCode("d21", "その他")));

        _maxId = 26;
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

    public void SaveAll(IEnumerable<WorkCodeFamily> families)
    {
        _codeFamilies.Clear();
        _codeFamilies.AddRange(families);
    }
}