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
        _colorDictionary.Add("F6", "#cc6601");
        _colorDictionary.Add("F7", "#cc6602");
        _colorDictionary.Add("F8", "#cc6603");
        _colorDictionary.Add("F9", "#cc6604");
        _colorDictionary.Add("F10", "#cc6605");
        _colorDictionary.Add("F11", "#cc6606");
        _colorDictionary.Add("F12", "#cc6607");
        _colorDictionary.Add("F13", "#cc6608");
        _colorDictionary.Add("F14", "#cc6609");
        _colorDictionary.Add("F15", "#cc6610");
        _colorDictionary.Add("F16", "#cc6611");
        _colorDictionary.Add("F17", "#cc6612");
        _colorDictionary.Add("F18", "#cc6613");
        _colorDictionary.Add("F19", "#cc6614");
        _colorDictionary.Add("F20", "#cc6615");
        _colorDictionary.Add("F21", "#cc6616");
        _colorDictionary.Add("F22", "#cc6617");
        _colorDictionary.Add("F23", "#cc6618");
        _colorDictionary.Add("F24", "#cc6619");
        _colorDictionary.Add("F25", "#cc6620");
        _colorDictionary.Add("F26", "#cc6621");
    }

    public string GetHtmlColorCodeById(string workCodeFamilyId)
    {
        string colorCode;
        if (_colorDictionary.TryGetValue(workCodeFamilyId, out colorCode))
        {
            return colorCode;
        }
        return "#c0c0c0";
    }

    public void Register(string id, string colorCode)
    {
        if (_colorDictionary.ContainsKey(id))
        {
            _colorDictionary[id] = colorCode;
        }
        else
        {
            _colorDictionary.Add(id, colorCode);
        }
    }
}