namespace MyWorkDashboard.Shared.ToDoTasks;

public class ToDoItem
{
    public string Id { get; set; }
    public DateOnly DueDate { get; set; }
    public string Description { get; set; }
    public ToDoTaskDetail Detail { get; set; }

    public ToDoItem(string id, DateOnly dueDate, string description)
    {
        Id = id;
        DueDate = dueDate;
        Description = description;
        Detail = new ToDoTaskDetail();
    }

    public ToDoItem DeepCopy()
    {
        return new ToDoItem(Id, DueDate, Description) { Detail = Detail.DeepCopy() };
    }
}