using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWorkDashboard.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWorkDashboard.Shared.Mock;

namespace MyWorkDashboard.Shared.Services.Tests
{
    [TestClass()]
    public class SchedulingServiveTests
    {

        [TestMethod()]
        public void GetFreeTimeSpansTest1()
        {
            var repo = new MockDutyRepository();
            repo.DeleteAll();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            SchedulingServive servive = new SchedulingServive(repo, new MockWorkCodeFamilyRepository(), new MockDutyColorRepository(), new MockToDoRepository(), new MockPreferenceRepository());

            var freeTimes = servive.GetFreeTimeSpans(today).ToArray();

            Assert.AreEqual(1, freeTimes.Length);
            Assert.AreEqual(new TimeOnly(0, 0), freeTimes[0].StartTime);
            Assert.AreEqual(new TimeOnly(23, 59), freeTimes[0].EndTime);
        }

        [TestMethod()]
        public void GetFreeTimeSpansTest2()
        {
            var repo = new MockDutyRepository();
            repo.DeleteAll();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            repo.AppendNew(today, "09:00", "10:00", "タスク1");
            SchedulingServive servive = new SchedulingServive(repo, new MockWorkCodeFamilyRepository(), new MockDutyColorRepository(), new MockToDoRepository(), new MockPreferenceRepository());

            var freeTimes = servive.GetFreeTimeSpans(today).ToArray();

            Assert.AreEqual(2, freeTimes.Length);
            Assert.AreEqual(new TimeOnly(0, 0), freeTimes[0].StartTime);
            Assert.AreEqual(new TimeOnly(9, 0), freeTimes[0].EndTime);
            Assert.AreEqual(new TimeOnly(10, 0), freeTimes[1].StartTime);
            Assert.AreEqual(new TimeOnly(23, 59), freeTimes[1].EndTime);
        }


        [TestMethod()]
        public void GetFreeTimeSpansTest3()
        {
            var repo = new MockDutyRepository();
            repo.DeleteAll();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            repo.AppendNew(today, "09:00", "10:00", "タスク1");
            repo.AppendNew(today, "10:00", "12:00", "タスク2");
            repo.AppendNew(today, "13:00", "14:00", "タスク3");
            SchedulingServive servive = new SchedulingServive(repo, new MockWorkCodeFamilyRepository(), new MockDutyColorRepository(), new MockToDoRepository(), new MockPreferenceRepository());

            var freeTimes = servive.GetFreeTimeSpans(today).ToArray();

            Assert.AreEqual(3, freeTimes.Length);
            Assert.AreEqual(new TimeOnly(0, 0), freeTimes[0].StartTime);
            Assert.AreEqual(new TimeOnly(9, 0), freeTimes[0].EndTime);
            Assert.AreEqual(new TimeOnly(12, 0), freeTimes[1].StartTime);
            Assert.AreEqual(new TimeOnly(13, 0), freeTimes[1].EndTime);
            Assert.AreEqual(new TimeOnly(14, 0), freeTimes[2].StartTime);
            Assert.AreEqual(new TimeOnly(23, 59), freeTimes[2].EndTime);
        }

        [TestMethod()]
        public void GetFreeTimeSpansTest4()
        {
            var repo = new MockDutyRepository();
            repo.DeleteAll();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            repo.AppendNew(today, "09:00", "10:00", "タスク1");
            repo.AppendNew(today, "11:00", "12:00", "タスク2");
            repo.AppendNew(today, "09:30", "11:30", "タスク3"); //タスク1と2にまたがっている（不整合）
            SchedulingServive servive = new SchedulingServive(repo, new MockWorkCodeFamilyRepository(), new MockDutyColorRepository(), new MockToDoRepository(), new MockPreferenceRepository());

            var freeTimes = servive.GetFreeTimeSpans(today).ToArray();

            Assert.AreEqual(2, freeTimes.Length);
            Assert.AreEqual(new TimeOnly(0, 0), freeTimes[0].StartTime);
            Assert.AreEqual(new TimeOnly(9, 0), freeTimes[0].EndTime);
            Assert.AreEqual(new TimeOnly(12, 0), freeTimes[1].StartTime);
            Assert.AreEqual(new TimeOnly(23, 59), freeTimes[1].EndTime);

        }

        [TestMethod()]
        public void GetFreeTimeSpansTest5()
        {
            var repo = new MockDutyRepository();
            repo.DeleteAll();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            repo.AppendNew(today, "09:00", "10:00", "タスク1");
            repo.AppendNew(today, "11:00", "12:00", "タスク2");
            repo.AppendNew(today, "09:30", "10:30", "タスク3"); //タスク1にまたがっている（不整合）
            SchedulingServive servive = new SchedulingServive(repo, new MockWorkCodeFamilyRepository(), new MockDutyColorRepository(), new MockToDoRepository(), new MockPreferenceRepository());

            var freeTimes = servive.GetFreeTimeSpans(today).ToArray();

            Assert.AreEqual(3, freeTimes.Length);
            Assert.AreEqual(new TimeOnly(0, 0), freeTimes[0].StartTime);
            Assert.AreEqual(new TimeOnly(9, 0), freeTimes[0].EndTime);
            Assert.AreEqual(new TimeOnly(10, 30), freeTimes[1].StartTime);
            Assert.AreEqual(new TimeOnly(11, 0), freeTimes[1].EndTime);
            Assert.AreEqual(new TimeOnly(12, 0), freeTimes[2].StartTime);
            Assert.AreEqual(new TimeOnly(23, 59), freeTimes[2].EndTime);

        }
    }
}