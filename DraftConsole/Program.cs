

using MyWorkDashboard.Shared.Services;
using WorkBord;
using WorkBord.Duties;
using WorkBord.WorkCodeFamilies;


SchedulingServive servive = new SchedulingServive(new MockDutyRepository(), new MockWorkCodeFamilyRepository(), new MockDutyColorRepository());

var arr = servive.GetFreeTimeSpans(DateOnly.FromDateTime(DateTime.Now)).ToArray();


Console.ReadKey();