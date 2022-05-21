using WorkBord.Duties;

namespace WorkBord;

public class MockDutyRepository : IDutyRepository
{
    readonly List<Duty> _duties;

    public MockDutyRepository()
    {
        _duties = new List<Duty>();
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        Register(new BusinessDuty("D1", today, new WorkTimeRange(new TimeOnly(10, 0), 60), new WorkTask("コーディング", ""), "F1"));
        Register(new BusinessDuty("D2", today, new WorkTimeRange(new TimeOnly(11, 0), 60), new WorkTask("打ち合わせ", ""), "F2"));
        Register(new BusinessDuty("D3", today, new WorkTimeRange(new TimeOnly(13, 0), 60), new WorkTask("検証作業", ""), "F3"));
        Register(new BusinessDuty("D4", today, new WorkTimeRange(new TimeOnly(14, 0), 60), new WorkTask("障害解析", ""), "F4"));
        Register(new BusinessDuty("D5", today, new WorkTimeRange(new TimeOnly(15, 0), 120), new WorkTask("キッティング", ""), "F5"));
    }

    public void Register(Duty duty)
    {
        // todo 時刻のかぶりチェック
        this.Delete(duty.Id);
        _duties.Add(duty);
    }

    public void Delete(string dutyId)
    {
        var found = _duties.FirstOrDefault(d => d.Id == dutyId);
        if (found != null)
        {
            _duties.Remove(found);
        }
    }

    public Duty? FindById(string dutyId)
    {
        return _duties.FirstOrDefault(d => d.Id == dutyId);
    }

    public Duty[] FindByDate(DateOnly date)
    {
        return _duties.Where(d => d.Date == date).ToArray();
    }
}