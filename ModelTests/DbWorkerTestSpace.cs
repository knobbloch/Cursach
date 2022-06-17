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
    public class DbWorkerTestSpace
    {
        [TestMethod]
        public void CreateInstanceOfDbWorkerTest()
        {
            DbWorker myDb = new("UserData.db");
        }
        [TestMethod]
        public void GetCardsListTest()
        {
            DbWorker myDb = new("UserData.db");
            var result = myDb.GetCards();
        }
        [TestMethod]
        public void AddExistingCardTest()
        {
            DbWorker myDb = new("UserData.db");
            var result = myDb.AddCard("TestNew4", 1400);
            Assert.IsTrue(result == ModelEditDataResultStates.ReturnCardState.ErrorTypeNameConstraint);
        }
    }
}
