namespace MyWorkDashboard.Shared.Duties;

public class WorkTask
{
    public string Title { get; set; }
    public string Description { get; set; }

    public WorkTask(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public override string ToString()
    {
        return $"{nameof(Title)}: {Title}, {nameof(Description)}: {Description}";
    }

    public WorkTask DeepCopy()
    {
        return new WorkTask(this.Title, this.Description);
    }
}