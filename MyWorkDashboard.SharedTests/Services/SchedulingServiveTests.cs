using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWorkDashboard.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkBord;

namespace MyWorkDashboard.Shared.Services.Tests
{
    [TestClass()]
    public class SchedulingServiveTests
    {
        [TestMethod()]
        public void GetFreeTimeSpansTest()
        {
            SchedulingServive servive = new SchedulingServive(new MockDutyRepository(), new MockWorkCodeFamilyRepository(), new MockDutyColorRepository());

        }
    }
}