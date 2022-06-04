using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using MyWorkDashboard.Shared;
using MyWorkDashboard.Shared.Services;
using MyWorkDashboard.Shared.WorkCodeFamilies;

namespace MyWorkDesktop.Services;

public class TsvWorkCodeFamilyRepository : IWorkCodeFamilyRepository, IDutyColorRepository
{
    public string TsvrFilePath { get; }

    private List<WorkCodeFamily> _codeFamilies;

    private List<WorkCodeFamily> WorkCodeFamilies
    {
        get
        {
            // 遅延ロード
            LazyLoadMaster();
            return _codeFamilies;
        }
    }

    private Dictionary<string, string> _colorDictionary;

    public Dictionary<string, string> ColorDictionary
    {
        get
        {
            // 遅延ロード
            LazyLoadMaster();
            return _colorDictionary;
        }
    }

    public TsvWorkCodeFamilyRepository(string tsvrFilePath)
    {
        TsvrFilePath = tsvrFilePath;
    }

    private void LazyLoadMaster()
    {
        if (_codeFamilies == null || _colorDictionary == null)
        {
            string dir = Path.GetDirectoryName(TsvrFilePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(TsvrFilePath)) File.WriteAllText(TsvrFilePath, "");

            (WorkCodeFamily family, string colorCode)[] items = ParseWorkCodeFamilies(TsvrFilePath).ToArray();
            _codeFamilies = items.Select(it => it.family).ToList();
            _colorDictionary = items.ToDictionary(it => it.family.Id, it => it.colorCode);
        }
    }

    private IEnumerable<(WorkCodeFamily family, string colorCode)> ParseWorkCodeFamilies(string path)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); // memo: Shift-JISを扱うためのおまじない
        using var reader = new StreamReader(path, Encoding.GetEncoding("shift_jis"));
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var arr = line.Split('\t');
            if (arr.Length < 6) continue;
            
            // FamilyId CategoryId CategoryName WorkCodeId WorkCodeName ColorCode のタブ区切りとする
            var family = new WorkCodeFamily(arr[0], new WorkCategory(arr[1], arr[2]), new WorkCode(arr[3], arr[4]));
            string colorCode = arr[5];
            yield return (family, colorCode);
        }

    }


    public string GetNewId()
    {
        // たぶん使わない
        throw new System.NotImplementedException();
    }

    public void Register(WorkCodeFamily workCodeFamily)
    {
        // たぶん使わない
        throw new System.NotImplementedException();
    }

    public void Delete(string familyId)
    {
        // たぶん使わない
        throw new System.NotImplementedException();
    }

    public WorkCodeFamily? FindById(string familyId)
    {
        return WorkCodeFamilies.FirstOrDefault(f => f.Id == familyId);
    }

    public WorkCodeFamily[] GetAll()
    {
        return WorkCodeFamilies.ToArray();
    }

    public string GetHtmlColorCodeById(string workCodeFamilyId)
    {
        return ColorDictionary[workCodeFamilyId];
    }
}