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
    public class DbWorkerFactExpenditureTests
    {
#pragma warning disable CA1416 // Validate platform compatibility
        PlanExpenditureVisible planExpenditure = new PlanExpenditure
        {
            BeginDate = DateTime.Now.Ticks,
            EndDate = DateTime.Now.AddDays(10).Ticks,
            BeginDateDateTime = DateTime.Now,
            EndDateDateTime = DateTime.Now.AddDays(10),
            ExpenditureCategory = "MyTestFactCategory",
            Sum = 1000,
            PlanExpenditureImagePath = "/somepath"
        };
#pragma warning restore CA1416 // Validate platform compatibility
        [TestMethod]
        public void AddFactExpenditureTest()
        {
            
            DbWorker dbWorker = new("UserData.db");
            var result = dbWorker.AddFactExpenditure("TestExp1", "MyTestFactCategory", 100, DateTime.Now, "testCard", planExpenditure);
            Assert.IsTrue(result == ModelEditDataResultStates.ReturnFactExpenditureState.Success);
        }
        [TestMethod]
        public void AddFactExpenditureNoDateNoCategoryTest()
        {
            DbWorker dbWorker = new("UserData.db");
            var result = dbWorker.AddFactExpenditure("TestExp2", "", 100, DateTime.Now, "", planExpenditure);
            Assert.IsTrue(result == ModelEditDataResultStates.ReturnFactExpenditureState.Success);
        }
        [TestMethod]
        public void AddFactExpenditureNewTest()
        {
            DbWorker dbWorker = new("UserData.db");
            var result = dbWorker.AddFactExpenditure("TestExp5", "MyTestFactCategory", 1234, DateTime.Now, "testCard", planExpenditure);
            Assert.IsTrue(result == ModelEditDataResultStates.ReturnFactExpenditureState.Success);
        }
        [TestMethod]
        public void GetFactExpendituresListTest()
        {
            DbWorker dbWorker = new("UserData.db");
            var result = dbWorker.GetFactExpenditures();
        }
    }
}
