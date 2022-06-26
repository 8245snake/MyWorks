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
    public event EventHandler ToDoItemDeleted;

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

    public Task<WorkCodeFamily[]> GetAllWorkCodeFamily()
    {
        return _workCodeFamilyRepository.GetAllAsync();
    }

    public Task<WorkCodeFamily?> FindWorkCodeFamilyById(string id)
    {
        return _workCodeFamilyRepository.FindByIdAsync(id);
    }

    public async Task<string> GetDutyColorCodeAsync(BusinessDuty? duty)
    {
        if (duty == null)
        {
            return "#cccccc";
        }
        return await _dutyColorRepository.GetHtmlColorCodeByIdAsync(duty.WorkCodeFamilyId);
    }

    public string GetWorkCodeFamilyColorCode(string famulyId)
    {
        return _dutyColorRepository.GetHtmlColorCodeById(famulyId);
    }

    public Task ChangeSelectedDutyAsync(Duty duty, object sender)
    {
        SelectedDuty = duty;
        SelectedDutyChanged?.Invoke(sender, EventArgs.Empty);
        return Task.CompletedTask;
    }

    public async Task RaiseDutyPropertyChangedAsync(object? sender)
    {
        await _dutyRepository.RegisterAsync(SelectedDuty);
        DutyPropertyChanged.Invoke(sender, EventArgs.Empty);
    }

    public async Task UpdateDutyAsync(Duty duty)
    {
        await _dutyRepository.RegisterAsync(duty);
    }

    public Task ChangeSelectedDateAsync(DateOnly? date, object sender)
    {
        SelectedDate = date;
        SelectedDateChanged?.Invoke(sender, EventArgs.Empty);
        return Task.CompletedTask;
    }



    public async Task<Duty[]> FindDutiesByDateAsync(DateOnly date)
    {
        return await _dutyRepository.FindByDateAsync(date);
    }


    public async Task<Duty> CreateNewDutyAsync(DateOnly date, WorkTimeRange range)
    {
        string dutyId = await _dutyRepository.GetNewIdAsync();
        var duty = new BusinessDuty(dutyId, date, range, new WorkTask("", ""), DefaultWorkCodeId);
        await _dutyRepository.RegisterAsync(duty);
        return duty;
    }

    public async Task<Duty> DuplicateDutyAsync(Duty original, DateOnly date, WorkTimeRange range)
    {
        string dutyId = await _dutyRepository.GetNewIdAsync();
        var duty = original.Duplicate(dutyId);
        duty.Date = date;
        duty.StartTime = range.StartTime;
        duty.EndTime = range.EndTime;
        await _dutyRepository.RegisterAsync(duty);
        return duty;
    }

    public async Task DeleteDutyAsync(string id)
    {
        await _dutyRepository.DeleteAsync(id);
        DutyDeleted.Invoke(this, EventArgs.Empty);
    }

    public async Task<Duty> AddNewScheduleAsync(DateOnly date)
    {

        // 9時以降で開いているところに入れる(12時は除く)

        DateOnly targetDate = date;
        TimeOnly start = new TimeOnly(9, 0);
        TimeOnly end = start.AddMinutes(60);

        // その日の中で最後に終わる業務
        var lastEndTime = (await this.FindDutiesByDateAsync(targetDate)).MaxBy(d => d.EndTime)?.EndTime;
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

        var createdDuty = await this.CreateNewDutyAsync(targetDate, new WorkTimeRange(start, end));
        return createdDuty;
    }

    public async Task<DutyStaticticResult[]> TakeStatisticsOfSelectedDayAsync()
    {
        if (SelectedDate == null) return new DutyStaticticResult[] { };
        var duties = await _dutyRepository.FindByDateAsync(this.SelectedDate.Value);
        DutiesOfDay dd = new DutiesOfDay(duties);
        return await dd.TakeStatistics(_workCodeFamilyRepository);
    }

    /// <summary>
    /// 指定した日のうち予定が入っていない期間を列挙する
    /// </summary>
    /// <param name="date">日付</param>
    /// <returns>予定が入っていない時間</returns>
    public IEnumerable<WorkTimeRange> GetFreeTimeSpans(DateOnly date)
    {
        var mergedRanges = GetMergedTimeRange(FindDutiesByDateAsync(date).Result).ToArray();
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

    public async Task<ToDoItem[]> FindToDoItemsByDate(DateOnly date)
    {
        return await _toDoRepository.FindByDateAsync(date);
    }

    public async Task<ToDoItem?> FindToDoItemsById(string id)
    {
        return await _toDoRepository.FindByIdAsync(id);
    }

    public async Task<ToDoItem> CreateNewToDoItem(DateOnly date)
    {
        string id = await _toDoRepository.GetNewIdAsync();
        ToDoItem item = new ToDoItem(id, date, "");
        await _toDoRepository.RegisterAsync(item);
        return item;
    }

    public async Task<ToDoItem> CreateNewToDoItem(Duty duty)
    {
        ToDoItem todoItem = await CreateNewToDoItem(duty.Date);
        todoItem.Description = duty.Title;
        todoItem.Detail.Comment = duty.Description;
        todoItem.Detail.Priority = 1;
        if (duty is BusinessDuty bd)
        {
            todoItem.Detail.WorkCodeFamilyId = bd.WorkCodeFamilyId;
        }

        return todoItem;
    }

    public async Task DeleteToDoItem(string id)
    {
        await _toDoRepository.DeleteAsync(id);
        ToDoItemDeleted?.Invoke(this, EventArgs.Empty);
    }

    public async Task UpdateToDoItem(ToDoItem item, object? sender)
    {
        await _toDoRepository.RegisterAsync(item);
        ToDoItemChanged?.Invoke(sender, EventArgs.Empty);
    }

    public async Task<int> MoveTodayOlderTodoItems()
    {
        int count = 0;
        var today = DateOnly.FromDateTime(DateTime.Now);
        foreach (ToDoItem item in await _toDoRepository.FindItemsBeforeThanAsync(today))
        {
            await _toDoRepository.DeleteAsync(item.Id);
            item.DueDate = today;
            await _toDoRepository.RegisterAsync(item);
            count++;
        }

        return count;
    }

    #endregion

    public async Task SaveAll(IEnumerable<WorkCodeFamily> items)
    {
        await _workCodeFamilyRepository.RegisterAllAsync(items);
    }

    public Task RegisterColorAsync(string id, string colorCode)
    {
        return _dutyColorRepository.RegisterAsync(id, colorCode);
    }

    public Task<string> GetNewWorkCodeIdAsync()
    {
        return _workCodeFamilyRepository.GetNewIdAsync();
    }
}