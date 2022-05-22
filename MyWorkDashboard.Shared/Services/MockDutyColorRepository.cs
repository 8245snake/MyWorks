namespace MyWorkDashboard.Shared.Services;

public class MockDutyColorRepository : IDutyColorRepository
{
    private Dictionary<string, string> _colorDictionary;

    public MockDutyColorRepository()
    {
        _colorDictionary = new Dictionary<string, string>();
        _colorDictionary.Add("F1", "#ff6600");
        _colorDictionary.Add("F2", "#ffcc33");
        _colorDictionary.Add("F3", "#99cc66");
        _colorDictionary.Add("F4", "#33ffcc");
        _colorDictionary.Add("F5", "#cc66ff");
    }

    public string GetHtmlColorCodeById(string workCodeFamilyId)
    {
        return _colorDictionary[workCodeFamilyId];
    }
}