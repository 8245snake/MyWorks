using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyWorkDashboard.Shared;
using MyWorkDashboard.Shared.UserPreferences;
using Newtonsoft.Json;

namespace MyWorkDesktop.Services;

public class JsonPreferenceRepository : IPreferenceRepository
{

    public string DataDirectory { get; }
    public string PreferenceFilePath => Path.Combine(DataDirectory, "preference.json");
    public string TemplatesFilePath => Path.Combine(DataDirectory, "templates.json");

    private readonly JsonSerializerSettings _settings;

    public JsonPreferenceRepository(string dataDirectory)
    {
        DataDirectory = dataDirectory;
        if (!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);
        _settings = new JsonSerializerSettings();
        _settings.Converters.Add(new DateOnlyJsonConverter());
        _settings.Converters.Add(new TimeOnlyJsonConverter());
        _settings.Converters.Add(new NullableDateOnlyJsonConverter());
        _settings.Converters.Add(new NullableTimeOnlyJsonConverter());
    }

    private ThemePreference GetThemePreference()
    {
        try
        {
            string jsonStr = File.ReadAllText(PreferenceFilePath);
            ThemePreferenceJson jsonObj = JsonConvert.DeserializeObject<ThemePreferenceJson>(jsonStr, _settings);
            return jsonObj.Create();
        }
        catch
        {
            //todo エラー処理
            return new ThemePreference();
        }
    }

    private void SaveThemePreference(ThemePreference preference)
    {
        try
        {
            ThemePreferenceJson data = new ThemePreferenceJson(preference);
            string jsonStr = JsonConvert.SerializeObject(data, Formatting.None, _settings);
            File.WriteAllText(PreferenceFilePath, jsonStr);
        }
        catch
        {
            // todo エラー処理
        }
    }

    public Task<ThemePreference> GetThemePreferenceAsync()
    {
        return Task.FromResult(GetThemePreference());
    }

    public Task SaveThemePreferenceAsync(ThemePreference preference)
    {
        SaveThemePreference(preference);
        return Task.CompletedTask;
    }

    public Task<DutyTemplate[]> GetAllDutyTemplatesAsync()
    {
        try
        {
            string jsonStr = File.ReadAllText(TemplatesFilePath);
            DutyTemplateJson[] jsonObj = JsonConvert.DeserializeObject<DutyTemplateJson[]>(jsonStr, _settings);
            return Task.FromResult(jsonObj.Select(t => t.Create()).ToArray());
        }
        catch
        {
            return Task.FromResult(new DutyTemplate[] { });
        }
    }

    public Task<string> GetNewId()
    {
        return Task.FromResult($"M{DateTime.Now.Ticks.ToString()}");
    }

    public async Task Update(DutyTemplate template)
    {
        if( template == null) return;

        try
        {
            // 全登録しかない
            var templates = (await GetAllDutyTemplatesAsync()).ToList();
            var index = templates.FindIndex(t => t.Id == template.Id);
            if (index < 0)
            {
                templates.Add(template);
            }
            else
            {
                templates.RemoveAt(index);
                templates.Insert(index, template);
            }

            await SaveAllTemplates(templates.ToArray());
        }
        catch
        {
        }
    }

    public Task SaveAllTemplates(DutyTemplate[] templates)
    {
        try
        {
            DutyTemplateJson[] data = templates.Select(t => new DutyTemplateJson(t)).ToArray();
            string jsonStr = JsonConvert.SerializeObject(data, Formatting.None, _settings);
            File.WriteAllText(TemplatesFilePath, jsonStr);
            return Task.CompletedTask;
        }
        catch(Exception ex)
        {
            return Task.FromException(ex);
        }
    }

    public Task Delete(string templateId)
    {
        throw new NotImplementedException();
    }
}



[JsonObject]
public class ThemePreferenceJson
{
    [JsonProperty("isDarkTheme")]
    public bool IsDarkTheme { get; set; }

    public ThemePreferenceJson()
    {
    }

    public ThemePreferenceJson(ThemePreference preference)
    {
        IsDarkTheme = preference.IsDarkTheme;
    }

    public ThemePreference Create()
    {
        return new ThemePreference() { IsDarkTheme = this.IsDarkTheme };
    }

}

[JsonObject]
public class DutyTemplateJson
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("menuName")]
    public string MenuName { get; set; }

    [JsonProperty("iconName")]
    public string IconName { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

      [JsonProperty("WorkCodeFamilyId")]
    public string? WorkCodeFamilyId { get; set; }

    [JsonProperty("minute")]
    public int Minute { get; set; }

    public DutyTemplateJson()
    {
    }

    public DutyTemplateJson(DutyTemplate template)
    {
        this.Id = template.Id;
        this.MenuName = template.MenuName;
        this.IconName = template.IconName;
        this.Title = template.Title;
        this.Description = template.Description;
        this.WorkCodeFamilyId = template.WorkCodeFamilyId;
        this.Minute = template.Minute;
    }

    public DutyTemplate Create()
    {
        return new DutyTemplate(this.Id, this.MenuName)
        {
            IconName = this.IconName,
            Title = this.Title,
            Description = this.Description,
            WorkCodeFamilyId = this.WorkCodeFamilyId,
            Minute = this.Minute,
        };
    }
}