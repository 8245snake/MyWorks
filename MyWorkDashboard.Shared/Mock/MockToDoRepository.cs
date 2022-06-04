using MyWorkDashboard.Shared.ToDoTasks;

namespace MyWorkDashboard.Shared.Mock;

public class MockToDoRepository : IToDoRepository
{

    private List<ToDoItem> Items;

    public MockToDoRepository()
    {
        Items = new List<ToDoItem>();
    }

    public string GetNewId()
    {
        return $"T{DateTime.Now.Ticks.ToString()}";
    }

    public void Register(ToDoItem item)
    {
        Delete(item.Id);
        Items.Add(item);
    }

    public void Delete(string id)
    {
        var found = Items.FirstOrDefault(item => item.Id == id);
        if (found != null)
        {
            Items.Remove(found);
        }
    }

    public ToDoItem? FindById(string id)
    {
        return Items.FirstOrDefault(item => item.Id == id);
    }

    public ToDoItem[] FindByDate(DateOnly date)
    {
        return Items.Where(item => item.DueDate == date).ToArray();
    }

    public ToDoItem[] FindItemsBeforeThan(DateOnly date)
    {
        return Items.Where(item => item.DueDate < date).ToArray();
    }
}