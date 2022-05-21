namespace WorkBord.WorkCodeFamilies;

public class WorkCodeFamily
{
    public string Id { get; }

    public string Description => $"{_workCategory.Name} > {_workCode.Id}({_workCode.Name})";

    private WorkCategory _workCategory;
    private WorkCode _workCode;

    public WorkCodeFamily(string id, WorkCategory workCategory, WorkCode workCode)
    {
        Id = id;
        _workCategory = workCategory;
        _workCode = workCode;
    }

    public override string ToString()
    {
        return $"{nameof(_workCategory)}: {_workCategory}, {nameof(_workCode)}: {_workCode}, {nameof(Id)}: {Id}";
    }
}