using MyWorkDashboard.Shared.Duties;
using MyWorkDashboard.Shared.Services;
using MyWorkDashboard.Shared.ToDoTasks;

namespace MyWorkDashboard.Shared.Components;

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

    public void Update()
    {
        _servive.UpdateToDoItem(Todo, this);
    }

    public void CreateTask()
    {
        if (_servive.SelectedDate == null) return;

        Duty created = _servive.AddNewSchedule(_servive.SelectedDate.Value);
        created.Title = Todo.Description;
        _servive.ChangeSelectedDuty(created, new object());
        _servive.RaiseDutyPropertyChanged(this);
        _servive.ChangeSelectedDuty(created, new object());
        DeleteTask();
    }

    public void DeleteTask()
    {
        _todoItems.Remove(this);
        _servive.DeleteToDoItem(Todo.Id);
    }

    public void SendTomorrow()
    {
        _todoItems.Remove(this);
        _servive.DeleteToDoItem(Todo.Id);

        Todo.DueDate = Todo.DueDate.AddDays(1);
        _servive.UpdateToDoItem(Todo, this);
    }

    public void Select()
    {
        foreach (ToDoItemVm item in _todoItems)
        {
            item.IsEditing = (item == this);
        }
    }
}