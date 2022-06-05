using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDashboard.Shared.Components;

public class WorkCodeItemVM
{
    public string Id { get; set; }
    public string Category { get; set; }
    public string WorkCode { get; set; }
    public string WorkName { get; set; }
    public string ColorCode { get; set; }

    private WorkCodeFamily _workCodeFamily;


    public WorkCodeItemVM(WorkCodeFamily workCodeFamily)
    {
        _workCodeFamily = workCodeFamily;
        Id = workCodeFamily.Id;
        Category = workCodeFamily.Category.Name;
        WorkCode = workCodeFamily.WorkCode.Id;
        WorkName = workCodeFamily.WorkCode.Name;

    }
}