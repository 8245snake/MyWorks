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
        var found = Items.FirstOrDefault(t => t.Id == item.Id);
        if (found != null)
        {
            // 順番を保持したいため
            var index = Items.IndexOf(found);
            Items[index] = item;
        }
        else
        {
            Items.Add(item);
        }
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

    public Task<string> GetNewIdAsync()
    {
        return Task.FromResult(GetNewId());
    }

    public Task RegisterAsync(ToDoItem item)
    {
        Register(item);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id)
    {
        Delete(id);
        return Task.CompletedTask;
    }

    public Task<ToDoItem?> FindByIdAsync(string id)
    {
        return Task.FromResult(FindById(id));
    }

    public Task<ToDoItem[]> FindByDateAsync(DateOnly date)
    {
        return Task.FromResult(FindByDate(date));
    }

    public Task<ToDoItem[]> FindItemsBeforeThanAsync(DateOnly date)
    {
        return Task.FromResult(FindItemsBeforeThan(date));
    }
}