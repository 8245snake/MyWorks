namespace MyWorkDashboard.Shared.ToDoTasks;

public class ToDoTaskDetail
{
    public string? Comment { get; set; }
    public string? WorkCodeFamilyId { get; set; }
    public int? Priority { get; set; }

    public ToDoTaskDetail DeepCopy()
    {
        return new ToDoTaskDetail()
        {
            Comment = Comment,
            WorkCodeFamilyId = WorkCodeFamilyId,
            Priority = Priority,
        };
    }
}