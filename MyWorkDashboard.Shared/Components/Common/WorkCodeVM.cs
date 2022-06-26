using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDashboard.Shared;

public class WorkCodeVM
{
    public string Label { get; set; }
    public WorkCodeFamily Value { get; set; }

    public WorkCodeVM(WorkCodeFamily value)
    {
        Value = value;
        Label = $"{value.Category.Id}:{value.Category.Name} {value.WorkCode.Id} ({value.WorkCode.Name})";
    }
}