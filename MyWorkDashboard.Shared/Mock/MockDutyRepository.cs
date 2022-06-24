using MyWorkDashboard.Shared.Duties;

namespace MyWorkDashboard.Shared.Mock;

public class MockDutyRepository : IDutyRepository
{
    readonly List<Duty> _duties;
    private int _maxId = 0;

    public MockDutyRepository()
    {
        _duties = new List<Duty>();
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        _duties.Add(new BusinessDuty("D1", today, new WorkTimeRange(new TimeOnly(10, 0), 60), new WorkTask("コーディング", "もくもくと実装した"), "F1"));
        _duties.Add(new BusinessDuty("D2", today, new WorkTimeRange(new TimeOnly(11, 0), 60), new WorkTask("打ち合わせ", "リモートでレビューした"), "F2"));
        _duties.Add(new BusinessDuty("D3", today, new WorkTimeRange(new TimeOnly(13, 0), 60), new WorkTask("検証作業", "テストしてた"), "F3"));
        _duties.Add(new BusinessDuty("D4", today, new WorkTimeRange(new TimeOnly(14, 0), 60), new WorkTask("障害解析", "いろいろ調べてた"), "F4"));
        _duties.Add(new BusinessDuty("D5", today, new WorkTimeRange(new TimeOnly(15, 0), 120), new WorkTask("キッティング", "サーバーを作ってた"), "F5"));
        _maxId = 5;
    }

    public string GetNewId()
    {
        _maxId++;
        return $"D{_maxId}";
    }

    public void Register(Duty duty)
    {
        Utility.Log(duty);
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
#if DEBUG
        Console.WriteLine($"FindById dutyId={dutyId}");
#endif
        return _duties.FirstOrDefault(d => d.Id == dutyId);
    }

    public Duty[] FindByDate(DateOnly date)
    {
        return _duties.Where(d => d.Date == date).ToArray();
    }

    public Task<string> GetNewIdAsync()
    {
        return Task.FromResult(GetNewId());
    }

    public Task RegisterAsync(Duty duty)
    {
        this.Register(duty);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string dutyId)
    {
        this.Delete(dutyId);
        return Task.CompletedTask;
    }

    public Task<Duty?> FindByIdAsync(string dutyId)
    {
        return Task.FromResult(FindById(dutyId));
    }

    public Task<Duty[]> FindByDateAsync(DateOnly date)
    {
        return Task.FromResult(FindByDate(date));
    }

    #region テスト用

    public void DeleteAll()
    {
        _duties.Clear();
    }

    public void AppendNew(DateOnly date, string fromTime, string toTime, string summary)
    {
        var id = GetNewId();
        var model = new BusinessDuty(id, date, 
            new WorkTimeRange(TimeOnly.ParseExact(fromTime, "HH:mm"), TimeOnly.ParseExact(toTime, "HH:mm")),
            new WorkTask(summary, ""), "F1");

        _duties.Add(model);
    }


    #endregion
}