namespace WorkBord.WorkCodeFamilies;

public class WorkCode
{
    public string Id { get; }
    public string Name { get; }

    public WorkCode(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
    }
}