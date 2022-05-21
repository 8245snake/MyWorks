

using WorkBord;
using WorkBord.Duties;
using WorkBord.WorkCodeFamilies;

IWorkCodeFamilyRepository codeRepo = new MockWorkCodeFamilyRepository();
foreach (WorkCodeFamily workCodeFamily in codeRepo.GetAll())
{
    Console.WriteLine(workCodeFamily);
}

IDutyRepository dutyRepo = new MockDutyRepository();
foreach (Duty duty in dutyRepo.FindByDate(DateOnly.FromDateTime(DateTime.Now)))
{
    Console.WriteLine(duty);
}

Console.ReadKey();