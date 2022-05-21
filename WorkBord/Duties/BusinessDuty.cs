namespace WorkBord.Duties;

public class BusinessDuty : Duty
{
    public string WorkCodeFamilyId { get; }


    public BusinessDuty(string id, DateOnly date, WorkTimeRange timeRange, WorkTask workTask, string workCodeFamilyId) 
        : base(id, date, timeRange, workTask)
    {
        this.WorkCodeFamilyId = workCodeFamilyId;
    }
}