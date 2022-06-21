using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMishaLibrary.DbEntities;
using WpfMishaLibrary.VisibleEntities;

namespace WpfMishaLibrary
{
    public interface IModel
    {
        public ModelEditDataResultStates.ReturnCardState AddCard(string cardName, double balance);
        public ModelEditDataResultStates.ReturnFactExpenditureState AddFactExpenditure(string expenditureName, double sum, DateTime date, PlanExpenditureVisible planExpenditure, CardVisible card);
        public ModelEditDataResultStates.ReturnFactIncomeState AddFactIncome(string incomeName, double sum, DateTime date, PlanIncomeVisible planIncome, CardVisible card);
        public ModelEditDataResultStates.ReturnPlanExpenditureState AddPlanExpenditure(string expenditureCategory, double sum, DateTime beginDate, DateTime endDate, string planExpenditureImagePath);
        public ModelEditDataResultStates.ReturnPlanIncomeState AddPlanIncome(string incomeCategory, double sum, DateTime beginDate, DateTime endDate, string planIncomeImagePath);
        // Returns all records from tables
        public List<CardVisible> GetCards();
        public List<FactExpenditureVisible> GetFactExpenditures();
        public List<FactIncomeVisible> GetFactIncomes();
        public List<PlanExpenditureVisible> GetPlanExpenditures();
        public List<PlanIncomeVisible> GetPlanIncomes();
        // Returns specified by date records
        public List<PlanExpenditureVisible> GetPlanExpendituresSelectedDay(DateTime day);
        public List<PlanIncomeVisible> GetPlanIncomesSelectedDay(DateTime day);
        public List<PlanExpenditureVisible> GetPlanExpendituresDiapason(DateTime start, DateTime end);
        public List<PlanIncomeVisible> GetPlanIncomesDiapason(DateTime start, DateTime end);
        public List<FactExpenditureVisible> GetFactExpendituresDiapason(DateTime start, DateTime end);
        public List<FactIncomeVisible> GetFactIncomesDiapason(DateTime start, DateTime end);
    }
}
