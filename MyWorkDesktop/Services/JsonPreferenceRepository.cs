using System;
using System.IO;
using System.Threading.Tasks;
using MyWorkDashboard.Shared;
using MyWorkDashboard.Shared.UserPreferences;
using Newtonsoft.Json;

namespace MyWorkDesktop.Services;

public class JsonPreferenceRepository : IPreferenceRepository
{

    public string DataDirectory { get; }
    public string PreferenceFilePath => Path.Combine(DataDirectory, "preference.json");


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
        return Task.FromResult(new DutyTemplate[]
        {
            new DutyTemplate("1", "なんか1"),
            new DutyTemplate("2", "なんか2"),
            new DutyTemplate("3", "なんか3"),
        });
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