namespace MyWorkDashboard.Shared.WorkCodeFamilies;

public class WorkCategory
{
    public string Id { get; }
    public string Name { get; }

    public WorkCategory(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
    }
}