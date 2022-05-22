﻿using System.Data;
using WorkBord.WorkCodeFamilies;

namespace WorkBord.Duties;

public class DutiesOfDay
{
    private Duty[] _duties;

    public DutiesOfDay(IEnumerable<Duty> duties)
    {
        this._duties = duties.ToArray();
    }


    public DutyStaticticResult[] TakeStatistics(IWorkCodeFamilyRepository repository)
    {
        List<DutyStaticticResult> results = new List<DutyStaticticResult>();

        foreach (var gr in this._duties.OfType<BusinessDuty>().GroupBy(d=>d.WorkCodeFamilyId))
        {
            WorkCodeFamily family = repository.FindById(gr.Key);
            if (family == null)
            {
                throw new DataException($"データ不整合です。WorkCodeFamilyId={gr.Key} のデータは存在しません");
            }

            var items = gr.ToArray();
            var totalMinute = (int)items.Sum(d => (d.EndTime - d.StartTime).TotalMinutes);
            var result = new DutyStaticticResult(new TimeSpan(0, totalMinute, 0), family.Category, family.WorkCode, items);
            results.Add(result);
        }

        var breakResult = GetBreakResult();
        if (breakResult != null)
        {
            // 休憩時間も集計する
            results.Add(breakResult.Value);
        }

        return results.ToArray();
    }

    private DutyStaticticResult? GetBreakResult()
    {
        BreakDuty[] breakDuties = this._duties.OfType<BreakDuty>().ToArray();
        if (breakDuties.Length < 1)
        {
            return null;
        }

        var totalMinute = breakDuties.Sum(d => (d.EndTime - d.StartTime).Minutes);
        return new DutyStaticticResult(new TimeSpan(0, totalMinute, 0), breakDuties);
    }

}

public struct DutyStaticticResult
{
    public readonly TimeSpan Time;
    public readonly WorkCategory Category;
    public readonly WorkCode Code;
    public readonly bool IsBreakTime;
    public readonly Duty[] Duties;
    public DutyStaticticResult(TimeSpan time, Duty[] duties)
    {
        // 休憩時間
        IsBreakTime = true;
        Time = time;
        Duties = duties;
        Category = new WorkCategory("", "");
        Code = new WorkCode("", "");
    }

    public DutyStaticticResult(TimeSpan time, WorkCategory category, WorkCode code, Duty[] duties)
    {
        // 業務時間
        Time = time;
        Category = category;
        Code = code;
        Duties = duties;
        IsBreakTime = false;
    }


}