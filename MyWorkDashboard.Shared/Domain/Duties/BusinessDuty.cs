namespace MyWorkDashboard.Shared.Duties;

public class BusinessDuty : Duty
{
    public string WorkCodeFamilyId { get; set; }


    public BusinessDuty(string id, DateOnly date, WorkTimeRange timeRange, WorkTask workTask, string workCodeFamilyId) 
        : base(id, date, timeRange, workTask)
    {
        this.WorkCodeFamilyId = workCodeFamilyId;
    }

    public override Duty Duplicate(string newId)
    {
        return new BusinessDuty(newId, Date, this._timeRange.DeepCopy(), _workTask.DeepCopy(), this.WorkCodeFamilyId);
    }
}