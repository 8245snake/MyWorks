using WorkBord.WorkCodeFamilies;

namespace WorkBord;

public interface IWorkCodeFamilyRepository
{
    void Add(WorkCodeFamily workCodeFamily);
    void Remove(WorkCodeFamily workCodeFamily);
    WorkCodeFamily FindById(string WorkCodeFamilyId);
}