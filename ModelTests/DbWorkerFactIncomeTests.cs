using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfMishaLibrary;
using WpfMishaLibrary.DbEntities;
using WpfMishaLibrary.VisibleEntities;
namespace CursachTestProject
{
    [TestClass]
    // The imitation of the Presenter (MVP) class:
    // Calls DbWorker methods and gets results
    public class DbWorkerFactIncomeTests
    {
#pragma warning disable CA1416 // Validate platform compatibility
        PlanIncomeVisible planIncome = new PlanIncome
        {
            BeginDate = DateTime.Now.Ticks,
            EndDate = DateTime.Now.AddDays(10).Ticks,
            BeginDateDateTime = DateTime.Now,
            EndDateDateTime = DateTime.Now.AddDays(10),
            IncomeCategory = "MyTestFactCategory",
            Sum = 1000,
            PlanIncomeImagePath = "/somepath"
        };
#pragma warning restore CA1416 // Validate platform compatibility
        [TestMethod]
        public void AddFactIncomeTest()
        {
            DbWorker db = new("UserData.db");
            var result = db.AddFactIncome("testIncome", "MyTestIncomeCategory", 123, DateTime.Now, "testCard", planIncome);
            Assert.IsTrue(result == ModelEditDataResultStates.ReturnFactIncomeState.Success);
        }
        [TestMethod]
        public void GetFactIncomeTest()
        {
            DbWorker db = new("UserData.db");
            var result = db.GetFactIncomes();
            Assert.IsTrue(result.Count == 2);
        }
    }
}
