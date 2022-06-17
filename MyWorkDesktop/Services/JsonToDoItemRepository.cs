using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyWorkDashboard.Shared;
using MyWorkDashboard.Shared.ToDoTasks;
using Newtonsoft.Json;

namespace MyWorkDesktop.Services;

public class JsonToDoItemRepository : IToDoRepository
{
    public string DataDirectory { get; }
    private readonly JsonSerializerSettings _settings;

    public JsonToDoItemRepository(string dataDirectory)
    {
        DataDirectory = dataDirectory;
        if (!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);
        _settings = new JsonSerializerSettings();
        _settings.Converters.Add(new DateOnlyJsonConverter());
        _settings.Converters.Add(new TimeOnlyJsonConverter());
        _settings.Converters.Add(new NullableDateOnlyJsonConverter());
        _settings.Converters.Add(new NullableTimeOnlyJsonConverter());
    }

    public string GetNewId()
    {
        return $"T{DateTime.Now.Ticks.ToString()}";
    }

    public void Register(ToDoItem item)
    {
        try
        {
            string dir = GetDateFolderPath(item.DueDate);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            ToDoDataOfJson data = new ToDoDataOfJson(item);
            string jsonStr = JsonConvert.SerializeObject(data, Formatting.None, _settings);
            string targetFilePath = Path.Combine(dir, $"{item.Id}.json");
            File.WriteAllText(targetFilePath, jsonStr);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void Delete(string id)
    {
        try
        {
            var foundPath = FindJsonPathById(id);
            if (foundPath == null) return;
            File.Delete(foundPath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public ToDoItem FindById(string id)
    {
        try
        {
            var foundPath = FindJsonPathById(id);
            if (foundPath == null) return null;

            string jsonStr = File.ReadAllText(foundPath);
            ToDoDataOfJson jsonObj = JsonConvert.DeserializeObject<ToDoDataOfJson>(jsonStr, _settings);
            return jsonObj.Create();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public ToDoItem[] FindByDate(DateOnly date)
    {
        var dir = GetDateFolderPath(date);
        if (!Directory.Exists(dir)) return new ToDoItem[] { };

        return CreateTodoItems(dir).ToArray();
    }

    public ToDoItem[] FindItemsBeforeThan(DateOnly date)
    {
        var list = new List<ToDoItem>();
        var dirs = Directory.EnumerateDirectories(DataDirectory, "*")
            .Select(dir => DateOnly.ParseExact(Path.GetFileName(dir), "yyyyMMdd"))
            .Where(d => d < date).ToArray();

        foreach (DateOnly targetDate in dirs)
        {
            list.AddRange(this.FindByDate(targetDate));
        }

        return list.ToArray();
    }

    public Task<string> GetNewIdAsync()
    {
        return Task.FromResult(GetNewId());
    }

    public Task RegisterAsync(ToDoItem item)
    {
        Register(item);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id)
    {
        Delete(id);
        return Task.CompletedTask;
    }

    public Task<ToDoItem?> FindByIdAsync(string id)
    {
        return Task.FromResult(FindById(id));
    }

    public Task<ToDoItem[]> FindByDateAsync(DateOnly date)
    {
        return Task.FromResult(FindByDate(date));
    }

    public Task<ToDoItem[]> FindItemsBeforeThanAsync(DateOnly date)
    {
        return Task.FromResult(FindItemsBeforeThan(date));
    }

    private string FindJsonPathById(string dutyId)
    {
        string foundPath = Directory.EnumerateFiles(DataDirectory, "*.json", SearchOption.AllDirectories)
            .FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == dutyId);
        return foundPath;
    }

    private IEnumerable<ToDoItem> CreateTodoItems(string dir)
    {
        foreach (var file in Directory.EnumerateFiles(dir, "*.json"))
        {
            string jsonStr = File.ReadAllText(file);
            ToDoDataOfJson jsonObj = JsonConvert.DeserializeObject<ToDoDataOfJson>(jsonStr, _settings);
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
public class ToDoDataOfJson
{

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("dueDate")]
    public DateOnly DueDate { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }

    public ToDoDataOfJson()
    {
    }

    public ToDoDataOfJson(ToDoItem item)
    {
        this.Id = item.Id;
        this.DueDate = item.DueDate;
        this.Description = item.Description;
    }

    public ToDoItem Create()
    {
        return new ToDoItem(Id, DueDate, Description);
    }

}