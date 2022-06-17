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
        public ModelEditDataResultStates.ReturnFactExpenditureState AddFactExpenditure(string expenditureName, string factExpenditureCategory, double sum, DateTime date, string cardName, PlanExpenditureVisible planExpenditure);
        public ModelEditDataResultStates.ReturnFactIncomeState AddFactIncome(string incomeName, string factIncomeCategory, double sum, DateTime date, string cardName, PlanIncomeVisible planIncome);
        public ModelEditDataResultStates.ReturnPlanExpenditureState AddPlanExpenditure(string expenditureCategory, double sum, DateTime beginDate, DateTime endDate, string planExpenditureImagePath);
        public ModelEditDataResultStates.ReturnPlanIncomeState AddPlanIncome(string incomeCategory, double sum, DateTime beginDate, DateTime endDate, string planIncomeImagePath);
        public List<CardVisible> GetCards();
        public List<FactExpenditureVisible> GetFactExpenditures();
        public List<FactIncomeVisible> GetFactIncomes();
        public List<PlanExpenditureVisible> GetPlanExpenditures();
        public List<PlanIncomeVisible> GetPlanIncomes();
    }
}
