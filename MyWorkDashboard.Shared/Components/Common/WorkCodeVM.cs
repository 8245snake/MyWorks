using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDashboard.Shared;

public class WorkCodeVM
{
    public string Label { get; set; }
    public string Id { get; set; }

    public WorkCodeVM(WorkCodeFamily value)
    {
        Label = $"{value.Category.Id}:{value.Category.Name} {value.WorkCode.Id} ({value.WorkCode.Name})";
        Id = value.Id;
    }
}