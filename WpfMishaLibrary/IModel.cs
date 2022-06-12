using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMishaLibrary.DbEntities;

namespace WpfMishaLibrary
{
    public interface IModel
    {
        public ModelEditDataResultStates.ReturnCardState AddCard(string cardName, double balance);
        public ModelEditDataResultStates.ReturnFactExpenditureState AddFactExpenditure(string expenditureName, string factExpenditureCategory, double sum, DateTime date, string cardName);
        public ModelEditDataResultStates.ReturnFactIncomeState AddFactIncome(string incomeName, string factIncomeCategory, double sum, DateTime date, string cardName);
        public ModelEditDataResultStates.ReturnPlanExpenditureState AddPlanExpenditure(string expenditureCategory, double sum, DateTime beginDate, DateTime endDate, string planExpenditureImagePath);
        public bool AddPlanIncome(string incomeCategory, double sum, DateTime beginDate, DateTime endDate, string planIncomeImagePath);
        public List<Card> GetCards();
        public List<FactExpenditure> GetFactExpenditures();
        public List<FactIncome> GetFactIncomes();
        // return list of class of income, expenditure
    }
}
