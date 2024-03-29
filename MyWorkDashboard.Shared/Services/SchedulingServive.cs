﻿using System.Collections;
using System.Net.Http.Headers;
using MyWorkDashboard.Shared.Duties;
using MyWorkDashboard.Shared.Mock;
using MyWorkDashboard.Shared.Service;
using MyWorkDashboard.Shared.ToDoTasks;
using MyWorkDashboard.Shared.UserPreferences;
using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDashboard.Shared.Services;

public class SchedulingServive 
{
    #region イベント
    /// <summary> 選択中の業務が変化したときに発火する </summary>
    public event EventHandler? SelectedDutyChanged;

    /// <summary> 選択中の業務のプロパティが変化したときに発火する </summary>
    public event EventHandler? DutyPropertyChanged;

    /// <summary> 業務が削除されたときに発火する </summary>
    public event EventHandler? DutyDeleted;

    /// <summary>  選択中の日付が変化したときに発火する </summary>
    public event EventHandler? SelectedDateChanged;

    /// <summary>  選択中のToDoメモが変化したときに発火する </summary>
    public event EventHandler? ToDoItemChanged;

    /// <summary>  選択中のToDoメモが削除されたときに発火する </summary>
    public event EventHandler? ToDoItemDeleted;

    /// <summary>  作業コードのマスタが更新されたときに発火する </summary>
    public event EventHandler? WorkCodeUpdated;

    #endregion

    private readonly DutyService _dutyService;
    private readonly WorkCodeService _workCodeService;
    private readonly ToDoService _todoService;
    private readonly TemplateService _templateService;
    private readonly DutiesOfDay _dutiesOfDay;

    public Duty? SelectedDuty { get; private set; }
    public DateOnly? SelectedDate { get; private set; }

    public SchedulingServive()
     : this(new MockDutyRepository(), new MockWorkCodeFamilyRepository(), new MockDutyColorRepository(), new MockToDoRepository(), new MockPreferenceRepository())
    {
    }

    public SchedulingServive(IDutyRepository dutyRepository, IWorkCodeFamilyRepository workCodeFamilyRepository, IDutyColorRepository dutyColorRepository, IToDoRepository toDoRepository, IPreferenceRepository preferenceRepository)
    {
        _workCodeService = new WorkCodeService(workCodeFamilyRepository, dutyColorRepository);
        _dutyService = new DutyService(dutyRepository, _workCodeService.GetDefaultWorkCodeId());
        _todoService = new ToDoService(toDoRepository);
        _templateService = new TemplateService(preferenceRepository);
        _dutiesOfDay = new DutiesOfDay(workCodeFamilyRepository);
    }

    /// <summary>
    /// 選択中の日付を変更します
    /// </summary>
    /// <param name="date">日付</param>
    /// <param name="sender">イベント発行者</param>
    /// <remarks><seealso cref="SelectedDateChanged"/>が発火します</remarks>
    public Task ChangeSelectedDateAsync(DateOnly? date, object sender)
    {
        SelectedDate = date;
        SelectedDateChanged?.Invoke(sender, EventArgs.Empty);
        return Task.CompletedTask;
    }

    #region 作業コード

    public Task<WorkCodeFamily[]> GetAllWorkCodeFamily() => _workCodeService.GetAllWorkCodeFamily();
    public Task<WorkCodeFamily?> FindWorkCodeFamilyById(string id) => _workCodeService.FindWorkCodeFamilyById(id);
    public Task<string> GetDutyColorCodeAsync(BusinessDuty? duty) => _workCodeService.GetDutyColorCodeAsync(duty?.WorkCodeFamilyId);
    public string GetWorkCodeFamilyColorCode(string familyId) => _workCodeService.GetDutyColorCodeAsync(familyId).Result;
    public async Task SaveAll(IEnumerable<WorkCodeFamily> items)
    {
        await _workCodeService.SaveAll(items);
        WorkCodeUpdated?.Invoke(this, EventArgs.Empty);
    }

    public Task RegisterColorAsync(string id, string colorCode) => _workCodeService.RegisterColorAsync(id, colorCode);
    public Task<string> GetNewWorkCodeIdAsync() => _workCodeService.GetNewWorkCodeIdAsync();

    #endregion

    #region 業務データ

    public Task ChangeSelectedDutyAsync(Duty duty, object sender)
    {
        SelectedDuty = duty;
        SelectedDutyChanged?.Invoke(sender, EventArgs.Empty);
        return Task.CompletedTask;
    }

    public async Task RaiseDutyPropertyChangedAsync(object? sender)
    {
        await _dutyService.UpdateDutyAsync(SelectedDuty);
        DutyPropertyChanged?.Invoke(sender, EventArgs.Empty);
    }


    public Task UpdateDutyAsync(Duty duty) => _dutyService.UpdateDutyAsync(duty);

    public Task<Duty[]> FindDutiesByDateAsync(DateOnly date) => _dutyService.FindDutiesByDateAsync(date);

    public async Task<Duty> CreateNewDutyAsync(DateOnly date, WorkTimeRange range) => await _dutyService.CreateNewDutyAsync(date, range);

    public Task<Duty> DuplicateDutyAsync(Duty original, DateOnly date, WorkTimeRange range) => _dutyService.DuplicateDutyAsync(original, date, range);

    public Task<BusinessDuty?> CreateNewDutyFromTemplate(string templateId, DateTime startTime)
    {
        return _templateService.CreateNewDutyFromTemplate(templateId, startTime, _dutyService);
    }

    public async Task DeleteDutyAsync(string id)
    {
        await _dutyService.DeleteDutyAsync(id);
        DutyDeleted?.Invoke(this, EventArgs.Empty);
    }

    public Task<Duty> AddNewScheduleAsync(DateOnly date) => _dutyService.AddNewScheduleAsync(date);

    public async Task<DutyStaticticResult[]> TakeStatisticsOfSelectedDayAsync()
    {
        if (SelectedDate == null) return new DutyStaticticResult[] { };
        var duties = await _dutyService.FindDutiesByDateAsync(this.SelectedDate.Value);
        return await _dutiesOfDay.TakeStatistics(duties);
    }

    /// <summary>
    /// 指定した日のうち予定が入っていない期間を列挙する
    /// </summary>
    /// <param name="date">日付</param>
    /// <returns>予定が入っていない時間</returns>
    public IEnumerable<WorkTimeRange> GetFreeTimeSpans(DateOnly date) => _dutyService.GetFreeTimeSpans(date);

    /// <summary>
    /// 指定した日の期間において空いている期間を返す
    /// </summary>
    /// <param name="date">日付</param>
    /// <param name="start">検索開始時刻</param>
    /// <param name="end">検索終了時刻</param>
    /// <returns>空き時間の範囲</returns>
    public WorkTimeRange GetFreeRange(DateOnly date, TimeOnly start, TimeOnly end) => _dutyService.GetFreeRange(date, start, end);

    #endregion

    #region ToDoリスト

    public Task<ToDoItem[]> FindToDoItemsByDate(DateOnly date) => _todoService.FindToDoItemsByDate(date);

    public Task<ToDoItem?> FindToDoItemsById(string id) => _todoService.FindToDoItemsById(id);

    public Task<ToDoItem> CreateNewToDoItem(DateOnly date) => _todoService.CreateNewToDoItem(date);

    public Task<ToDoItem> CreateNewToDoItem(Duty duty) => _todoService.CreateNewToDoItem(duty);

    public async Task DeleteToDoItem(string id)
    {
        await _todoService.DeleteToDoItem(id);
        ToDoItemDeleted?.Invoke(this, EventArgs.Empty);
    }

    public async Task UpdateToDoItem(ToDoItem item, object? sender)
    {
        await _todoService.UpdateToDoItem(item);
        ToDoItemChanged?.Invoke(sender, EventArgs.Empty);
    }
    public Task<int> MoveTodayOlderTodoItems() => _todoService.MoveTodayOlderTodoItems();

    #endregion
}