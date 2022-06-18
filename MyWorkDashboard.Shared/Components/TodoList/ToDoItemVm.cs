using MyWorkDashboard.Shared.Duties;
using MyWorkDashboard.Shared.Services;
using MyWorkDashboard.Shared.ToDoTasks;

namespace MyWorkDashboard.Shared;

public class ToDoItemVm
{
    public ToDoItem Todo { get; }
    public bool IsEditing { get; set; }
    List<ToDoItemVm> _todoItems;
    private SchedulingServive _servive;

    public ToDoItemVm(ToDoItem todo, List<ToDoItemVm> items, SchedulingServive servive)
    {
        Todo = todo;
        IsEditing = false;
        _todoItems = items;
        _servive = servive;
    }

    public async Task Update()
    {
        await _servive.UpdateToDoItem(Todo, this);
    }

    public async Task CreateTask()
    {
        if (_servive.SelectedDate == null) return;

        Duty created = await _servive.AddNewScheduleAsync(_servive.SelectedDate.Value);

        created.Title = Todo.Description;
        await _servive.ChangeSelectedDutyAsync(created, new object());
        DeleteTask();
    }

    public async Task DeleteTask()
    {
        _todoItems.Remove(this);
        await _servive.DeleteToDoItem(Todo.Id);
    }

    public async Task SendTomorrow()
    {
        _todoItems.Remove(this);
        await _servive.DeleteToDoItem(Todo.Id);

        Todo.DueDate = Todo.DueDate.AddDays(1);
        await _servive.UpdateToDoItem(Todo, this);
    }

    public void Select()
    {
        foreach (ToDoItemVm item in _todoItems)
        {
            item.IsEditing = (item == this);
        }
    }
}