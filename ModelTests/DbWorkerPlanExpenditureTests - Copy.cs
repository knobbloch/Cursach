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
    public class DbWorkerPlanExpenditureTests
    {
        [TestMethod]
        public void AddPlanExpenditureTest()
        {
            DbWorker dbWorker = new("UserData.db");
            var result = dbWorker.AddPlanExpenditure("testPlanCategory", 20500, DateTime.Now, DateTime.Now.AddDays(10), "/anypath");
            Assert.IsTrue(result == ModelEditDataResultStates.ReturnPlanExpenditureState.Success);
        }
        [TestMethod]
        public void AddPlanExpenditureErrorTypeDateConstraintTest()
        {
            DbWorker dbWorker = new("UserData.db");
            var result = dbWorker.AddPlanExpenditure("testPlanCategory4", 20500, DateTime.Now, new DateTime(1, 1, 1), "/anypath");
            Assert.IsTrue(result == ModelEditDataResultStates.ReturnPlanExpenditureState.ErrorTypeDateConstraint);
        }
        [TestMethod]
        public void AddPlanExpenditureErrorTypeNameConstraintTest()
        {
            DbWorker dbWorker = new("UserData.db");
            var result = dbWorker.AddPlanExpenditure("testPlanCategory4", 20500, DateTime.Now, DateTime.Now.AddDays(10), "/anypath");
            Assert.IsTrue(result == ModelEditDataResultStates.ReturnPlanExpenditureState.ErrorTypeNameConstraint);
        }
        [TestMethod]
        public void GetPlanExpendituresListTest()
        {
            DbWorker dbWorker = new("UserData.db");
            var result = dbWorker.GetPlanExpenditures();
            Assert.IsTrue(result.Count == 1);
        }
    }
}
