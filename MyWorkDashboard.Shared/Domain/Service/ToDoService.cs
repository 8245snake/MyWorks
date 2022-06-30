using MyWorkDashboard.Shared.Duties;
using MyWorkDashboard.Shared.ToDoTasks;

namespace MyWorkDashboard.Shared.Service;

public class ToDoService
{
    private readonly IToDoRepository _toDoRepository;

    public ToDoService(IToDoRepository toDoRepository)
    {
        _toDoRepository = toDoRepository;
    }

    public async Task<ToDoItem[]> FindToDoItemsByDate(DateOnly date)
    {
        return await _toDoRepository.FindByDateAsync(date);
    }

    public async Task<ToDoItem?> FindToDoItemsById(string id)
    {
        return await _toDoRepository.FindByIdAsync(id);
    }

    public async Task<ToDoItem> CreateNewToDoItem(DateOnly date)
    {
        string id = await _toDoRepository.GetNewIdAsync();
        ToDoItem item = new ToDoItem(id, date, "");
        await _toDoRepository.RegisterAsync(item);
        return item;
    }

    public async Task<ToDoItem> CreateNewToDoItem(Duty duty)
    {
        ToDoItem todoItem = await CreateNewToDoItem(duty.Date);
        todoItem.Description = duty.Title;
        todoItem.Detail.Comment = duty.Description;
        todoItem.Detail.Priority = 1;
        if (duty is BusinessDuty bd)
        {
            todoItem.Detail.WorkCodeFamilyId = bd.WorkCodeFamilyId;
        }

        return todoItem;
    }

    public Task DeleteToDoItem(string id)
    {
        return _toDoRepository.DeleteAsync(id);
    }

    public Task UpdateToDoItem(ToDoItem item)
    {
        return _toDoRepository.RegisterAsync(item);
    }

    public async Task<int> MoveTodayOlderTodoItems()
    {
        int count = 0;
        var today = DateOnly.FromDateTime(DateTime.Now);
        foreach (ToDoItem item in await _toDoRepository.FindItemsBeforeThanAsync(today))
        {
            await _toDoRepository.DeleteAsync(item.Id);
            item.DueDate = today;
            await _toDoRepository.RegisterAsync(item);
            count++;
        }

        return count;
    }
}