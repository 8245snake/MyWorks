using MyWorkDashboard.Shared.Services;
using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDashboard.Shared;

public class WorkCodeItemVM
{
    public string Id
    {
        get { return _id; }
        set
        {
            _id = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string CategoryCode
    {
        get { return _categoryCode; }
        set
        {
            _categoryCode = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string CategoryName
    {
        get { return _categoryName; }
        set
        {
            _categoryName = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string WorkCode
    {
        get { return _workCode; }
        set
        {
            _workCode = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string WorkName
    {
        get { return _workName; }
        set
        {
            _workName = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string ColorCode
    {
        get { return _colorCode; }
        set
        {
            _colorCode = value;
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler PropertyChanged;

    private string _initialColorCode;
    private string _id;
    private string _categoryCode;
    private string _categoryName;
    private string _workCode;
    private string _workName;
    private string _colorCode;

    public WorkCodeItemVM(WorkCodeFamily workCodeFamily, SchedulingServive servive)
    {
        Id = workCodeFamily.Id;
        CategoryCode = workCodeFamily.Category.Id;
        CategoryName = workCodeFamily.Category.Name;
        WorkCode = workCodeFamily.WorkCode.Id;
        WorkName = workCodeFamily.WorkCode.Name;
        ColorCode = servive.GetWorkCodeFamilyColorCode(workCodeFamily.Id);
        _initialColorCode = ColorCode;

    }


    public WorkCodeFamily Create()
    {
        return new WorkCodeFamily(Id, new WorkCategory(CategoryCode, CategoryName), new WorkCode(WorkCode, WorkName));
    }

    public bool IsColorChanged
    {
        get { return _initialColorCode != ColorCode; }
    }
}