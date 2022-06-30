using MyWorkDashboard.Shared.Duties;

namespace MyWorkDashboard.Shared.Service;

public class DutyService
{
    private readonly IDutyRepository _dutyRepository;
    private readonly string _defaultWorkCodeId;

    public DutyService(IDutyRepository dutyRepository, string defaultWorkCodeId)
    {
        _dutyRepository = dutyRepository;
        _defaultWorkCodeId = defaultWorkCodeId;
    }

    public async Task<BusinessDuty> CreateNewDutyAsync(DateOnly date, WorkTimeRange range)
    {
        string dutyId = await _dutyRepository.GetNewIdAsync();
        BusinessDuty duty = new BusinessDuty(dutyId, date, range, new WorkTask("", ""), _defaultWorkCodeId);
        await _dutyRepository.RegisterAsync(duty);
        return duty;
    }

    /// <summary>
    /// Dutyを複製し、日付と時間を設定します。
    /// </summary>
    /// <param name="original">コピー元</param>
    /// <param name="date">日付</param>
    /// <param name="range">時間</param>
    /// <returns>複製されたDuty</returns>
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

    /// <summary>
    /// Dutyを削除します。
    /// </summary>
    /// <param name="id">削除対象の<seealso cref="Duty.Id"/></param>
    public async Task DeleteDutyAsync(string id)
    {
        await _dutyRepository.DeleteAsync(id);
    }

    /// <summary>
    /// 9時以降で空いている時間(12時は除く)にDutyを新規登録します
    /// </summary>
    /// <param name="date">日付</param>
    /// <returns>作成されたDuty</returns>
    public async Task<Duty> AddNewScheduleAsync(DateOnly date)
    {

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

    /// <summary>
    /// Dutyを更新します
    /// </summary>
    /// <param name="duty">更新対象のDuty</param>
    public async Task UpdateDutyAsync(Duty duty)
    {
        await _dutyRepository.RegisterAsync(duty);
    }

    /// <summary>
    /// 指定した日付のDutyを取得します
    /// </summary>
    /// <param name="date">日付</param>
    /// <returns>取得した配列</returns>
    public async Task<Duty[]> FindDutiesByDateAsync(DateOnly date)
    {
        return await _dutyRepository.FindByDateAsync(date);
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

    /// <summary>
    /// 隙間なくDuty繋がっている時間範囲を列挙します
    /// </summary>
    /// <param name="duties">その日のDuty配列</param>
    /// <returns>時間範囲</returns>
    private static IEnumerable<WorkTimeRange> GetMergedTimeRange(Duty[] duties)
    {
        WorkTimeRange lastRange = null;
        foreach (Duty duty in duties.OrderBy(d => d.StartTime))
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

    /// <summary>
    /// 指定した日の期間において空いている期間を返す
    /// </summary>
    /// <param name="date">日付</param>
    /// <param name="start">検索開始時刻</param>
    /// <param name="end">検索終了時刻</param>
    /// <returns>空き時間の範囲</returns>
    public WorkTimeRange GetFreeRange(DateOnly date, TimeOnly start, TimeOnly end)
    {
        var spanMinutes = (end - start).TotalMinutes;

        // 指定した時刻を含む空き時間を検索する
        var ranges = GetFreeTimeSpans(date).ToArray();
        WorkTimeRange? freeTimeRange = ranges.LastOrDefault(range => range.Contains(start) || range.Contains(end));

        if (freeTimeRange == null) return null;

        if (!freeTimeRange.Contains(start))
        {
            start = freeTimeRange.StartTime;
            end = start.AddMinutes(spanMinutes);
        }

        if (end > freeTimeRange.EndTime)
        {
            end = freeTimeRange.EndTime;
        }

        var range = new WorkTimeRange(start, end);
        return range;
    }

}