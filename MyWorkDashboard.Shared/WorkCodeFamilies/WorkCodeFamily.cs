namespace MyWorkDashboard.Shared.WorkCodeFamilies;

public class WorkCodeFamily
{
    public string Id { get; }

    public string Description => $"{Category.Name} > {WorkCode.Id}({WorkCode.Name})";

    public WorkCategory Category { get; }
    public WorkCode WorkCode { get; }

    public WorkCodeFamily(string id, WorkCategory category, WorkCode workCode)
    {
        Id = id;
        Category = category;
        WorkCode = workCode;
    }

    public override string ToString()
    {
        return $"{nameof(Category)}: {Category}, {nameof(WorkCode)}: {WorkCode}, {nameof(Id)}: {Id}";
    }
}