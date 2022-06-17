using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyWorkDashboard.Shared;
using MyWorkDashboard.Shared.Duties;
using Newtonsoft.Json;

namespace MyWorkDesktop.Services;

/// <summary>
/// JSONファイルにデータを保存している場合のリポジトリ。
/// ファイル名がId、フォルダが日付となるように保存すること。
/// </summary>
public class JsonDutyRepository : IDutyRepository
{
    public string DataDirectory { get; }

    private readonly JsonSerializerSettings _settings;

    public JsonDutyRepository(string dataDirectory)
    {
        DataDirectory = dataDirectory;
        if (!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);
        _settings = new JsonSerializerSettings();
        _settings.Converters.Add(new DateOnlyJsonConverter());
        _settings.Converters.Add(new TimeOnlyJsonConverter());
        _settings.Converters.Add(new NullableDateOnlyJsonConverter());
        _settings.Converters.Add(new NullableTimeOnlyJsonConverter());
    }

    private string GetNewId()
    {
        return $"D{DateTime.Now.Ticks.ToString()}";
    }

    private void Register(Duty duty)
    {
        try
        {
            string dir = GetDateFolderPath(duty.Date);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            DutyDataOfJson data = new DutyDataOfJson(duty);
            string jsonStr = JsonConvert.SerializeObject(data, Formatting.None, _settings);
            string targetFilePath = Path.Combine(dir, $"{duty.Id}.json");
            File.WriteAllText(targetFilePath, jsonStr);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void Delete(string dutyId)
    {
        try
        {
            var foundPath = FindJsonPathById(dutyId);
            if (foundPath == null) return;
            File.Delete(foundPath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private Duty FindById(string dutyId)
    {
        try
        {
            var foundPath = FindJsonPathById(dutyId);
            if (foundPath == null) return null;

            string jsonStr = File.ReadAllText(foundPath);
            DutyDataOfJson jsonObj = JsonConvert.DeserializeObject<DutyDataOfJson>(jsonStr, _settings);
            return jsonObj.Create();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private string FindJsonPathById(string dutyId)
    {
        string foundPath = Directory.EnumerateFiles(DataDirectory, "*.json", SearchOption.AllDirectories)
            .FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == dutyId);
        return foundPath;
    }

    private Duty[] FindByDate(DateOnly date)
    {
        var dir = GetDateFolderPath(date);
        if (!Directory.Exists(dir)) return new Duty[] { };

        return CreateDuties(dir).ToArray();
    }

    public Task<string> GetNewIdAsync()
    {
        return Task.FromResult(GetNewId());
    }

    public Task RegisterAsync(Duty duty)
    {
        Register(duty);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string dutyId)
    {
        Delete(dutyId);
        return Task.CompletedTask;
    }

    public Task<Duty> FindByIdAsync(string dutyId)
    {
        return Task.FromResult(FindById(dutyId));
    }

    public Task<Duty[]> FindByDateAsync(DateOnly date)
    {
        return Task.FromResult(FindByDate(date));
    }

    private IEnumerable<Duty> CreateDuties(string dir)
    {
        foreach (var file in Directory.EnumerateFiles(dir, "*.json"))
        {
            string jsonStr = File.ReadAllText(file);
            DutyDataOfJson jsonObj = JsonConvert.DeserializeObject<DutyDataOfJson>(jsonStr, _settings);
            yield return jsonObj.Create();
        }
    }

    private string GetDateFolderPath(DateOnly date)
    {
        string folderName = date.ToString("yyyyMMdd");
        return Path.Combine(DataDirectory, folderName);
    }
}

[JsonObject]
public class DutyDataOfJson
{

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("date")]
    public DateOnly Date { get; set; }

    [JsonProperty("startTime")]
    public TimeOnly? StartTime { get; set; }

    [JsonProperty("endTime")]
    public TimeOnly? EndTime { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("workCodeFamilyId")]
    public string? WorkCodeFamilyId { get; set; }

    public DutyDataOfJson()
    {
    }

    public DutyDataOfJson(Duty duty)
    {
        Id = duty.Id;
        Date = duty.Date;
        StartTime = duty.StartTime;
        EndTime = duty.EndTime;
        Summary = duty.Title;
        Description = duty.Description;
        if (duty is BusinessDuty bd)
        {
            WorkCodeFamilyId = bd.WorkCodeFamilyId;
        }
    }

    public Duty Create()
    {
        if (string.IsNullOrWhiteSpace(this.WorkCodeFamilyId))
        {
            return new BreakDuty(Id, Date, new WorkTimeRange(StartTime.Value, EndTime.Value), new WorkTask(Summary, Description));
        }
        else
        {
            return new BusinessDuty(Id, Date, new WorkTimeRange(StartTime.Value, EndTime.Value), new WorkTask(Summary, Description), WorkCodeFamilyId);
        }
    }

}