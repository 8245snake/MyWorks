using System.ComponentModel;
using MyWorkDashboard.Shared.Components;
using MyWorkDashboard.Shared.Duties;
using MyWorkDashboard.Shared.Mock;
using MyWorkDashboard.Shared.ToDoTasks;
using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDashboard.Shared.Services;

public class SchedulingServive 
{

    public event EventHandler SelectedDutyChanged;
    public event EventHandler DutyPropertyChanged;
    public event EventHandler DutyDeleted;
    public event EventHandler SelectedDateChanged;
    public event EventHandler ToDoItemChanged;

    private readonly IDutyRepository _dutyRepository;
    private readonly IWorkCodeFamilyRepository _workCodeFamilyRepository;
    private readonly IDutyColorRepository _dutyColorRepository;
    private readonly IToDoRepository _toDoRepository;

    private string DefaultWorkCodeId
    {
        get
        {
            if (_defaultWorkCodeId == null)
            {
                // とりあえず適当に先頭を返すことにする
                _defaultWorkCodeId = _workCodeFamilyRepository.GetAll().First().Id;
            }

            return _defaultWorkCodeId;
        }
    }

    private string? _defaultWorkCodeId;

    public Duty SelectedDuty { get; private set; }
    public DateOnly? SelectedDate { get; private set; }

    public SchedulingServive()
    {
        _dutyRepository = new MockDutyRepository();
        _workCodeFamilyRepository = new MockWorkCodeFamilyRepository();
        _dutyColorRepository = new MockDutyColorRepository();
        _toDoRepository = new MockToDoRepository();
    }

    public SchedulingServive(IDutyRepository dutyRepository, IWorkCodeFamilyRepository workCodeFamilyRepository, IDutyColorRepository dutyColorRepository, IToDoRepository toDoRepository)
    {
        _dutyRepository = dutyRepository;
        _workCodeFamilyRepository = workCodeFamilyRepository;
        _dutyColorRepository = dutyColorRepository;
        _toDoRepository = toDoRepository;
    }

    public WorkCodeFamily[] GetAllWorkCodeFamily()
    {
        return _workCodeFamilyRepository.GetAll();
    }

    public string GetWorkCodeFamilyColorCode(string dutyId)
    {
        var duty = _dutyRepository.FindById(dutyId) as BusinessDuty;
        if (duty == null)
        {
            return "#cccccc";
        }
        return _dutyColorRepository.GetHtmlColorCodeById(duty.WorkCodeFamilyId);
    }

    public void ChangeSelectedDuty(Duty duty, object sender)
    {
        SelectedDuty = duty;
        SelectedDutyChanged?.Invoke(sender, EventArgs.Empty);
    }

    public void ChangeSelectedDate(DateOnly? date, object sender)
    {
        SelectedDate = date;
        SelectedDateChanged?.Invoke(sender, EventArgs.Empty);
    }

    public void RaiseDutyPropertyChanged(object? sender)
    {
        _dutyRepository.Register(SelectedDuty);
        DutyPropertyChanged.Invoke(sender, EventArgs.Empty);
    }

    public Duty[] FindDutiesByDate(DateOnly date)
    {
        return _dutyRepository.FindByDate(date).ToArray();
    }

    public Duty CreateNewDuty(DateOnly date, TimeOnly start, TimeOnly end)
    {
        string dutyId = _dutyRepository.GetNewId();
        var duty = new BusinessDuty(dutyId, date, new WorkTimeRange(start, end), new WorkTask("", ""), DefaultWorkCodeId);
        _dutyRepository.Register(duty);
        return duty;
    }

    public void DeleteDuty(string id)
    {
        _dutyRepository.Delete(id);
        DutyDeleted.Invoke(this, EventArgs.Empty);
    }

    public Duty AddNewSchedule(DateOnly date)
    {

        // 9時以降で開いているところに入れる(12時は除く)

        DateOnly targetDate = date;
        TimeOnly start = new TimeOnly(9, 0);
        TimeOnly end = start.AddMinutes(60);

        // その日の中で最後に終わる業務
        var lastEndTime = this.FindDutiesByDate(targetDate).MaxBy(d => d.EndTime)?.EndTime;
        if (lastEndTime != null)
        {
            if (lastEndTime.Value.Hour != 12)
            {
                start = lastEndTime.Value;
            }
            else
            {
                start = new TimeOnly(13, 0);
            }
            end = start.AddMinutes(60);
        }

        var createdDuty = this.CreateNewDuty(targetDate, start, end);
        return createdDuty;
    }

    public DutyStaticticResult[]  TakeStatisticsOfSelectedDay()
    {
        if (SelectedDate == null) return new DutyStaticticResult[]{};
        DutiesOfDay dd = new DutiesOfDay(_dutyRepository.FindByDate(this.SelectedDate.Value));
        return dd.TakeStatistics(_workCodeFamilyRepository);
    }

    /// <summary>
    /// 指定した日のうち予定が入っていない期間を列挙する
    /// </summary>
    /// <param name="date">日付</param>
    /// <returns>予定が入っていない時間</returns>
    public IEnumerable<WorkTimeRange> GetFreeTimeSpans(DateOnly date)
    {
        var mergedRanges = GetMergedTimeRange(FindDutiesByDate(date)).ToArray();
        if (mergedRanges.Length == 0)
        {
            yield return new WorkTimeRange(new TimeOnly(0, 0), new TimeOnly(23, 59));
            yield break;
        }

        var lastTime = new TimeOnly(0, 0);
        foreach (var mergedRange in mergedRanges)
        {
            var range = new WorkTimeRange(lastTime, mergedRange.StartTime);
            if (range.Span.TotalMinutes > 0)
            {
                yield return range;
            }

            lastTime = mergedRange.EndTime;
        }

        var latestRange = new WorkTimeRange(lastTime, new TimeOnly(23, 59));
        if (latestRange.Span.Minutes > 0)
        {
            yield return latestRange;
        }

    }

    private static IEnumerable<WorkTimeRange> GetMergedTimeRange(Duty[] duties)
    {
        WorkTimeRange lastRange = null;
        foreach (Duty duty in duties.OrderBy(d=>d.StartTime))
        {
            if (lastRange == null)
            {
                lastRange = new WorkTimeRange(duty.StartTime, duty.EndTime);
                continue;
            }

            // つながっているか
            bool isNeighboring = (duty.StartTime <= lastRange.EndTime);
            if (isNeighboring)
            {
                lastRange.EndTime = duty.EndTime > lastRange.EndTime ? duty.EndTime : lastRange.EndTime;
                continue;
            }

            yield return lastRange;

            lastRange = new WorkTimeRange(duty.StartTime, duty.EndTime);
        }

        if (lastRange != null)
        {
            yield return lastRange;
        }
    }

    #region ToDoリスト

    public ToDoItem[] FindToDoItemsByDate(DateOnly date)
    {
        return _toDoRepository.FindByDate(date);
    }

    public ToDoItem CreateNewToDoItem(DateOnly date)
    {
        string id = _toDoRepository.GetNewId();
        ToDoItem item = new ToDoItem(id, date, "");
        _toDoRepository.Register(item);
        return item;
    }

    public void DeleteToDoItem(string id)
    {
        _toDoRepository.Delete(id);
    }

    public void UpdateToDoItem(ToDoItem item, object? sender)
    {
        _toDoRepository.Register(item);
        ToDoItemChanged?.Invoke(sender, EventArgs.Empty);
    }

    public int MoveTodayOlderTodoItems()
    {
        int count = 0;
        var today = DateOnly.FromDateTime(DateTime.Now);
        foreach (ToDoItem item in _toDoRepository.FindItemsBeforeThan(today))
        {
            _toDoRepository.Delete(item.Id);
            item.DueDate = today;
            _toDoRepository.Register(item);
            count++;
        }

        return count;
    }

    #endregion
}