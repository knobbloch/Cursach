using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMishaLibrary
{
    public interface IModel
    {
        public ModelEditDataResultStates.ReturnCardState AddCard(string cardName, double balance);
        public ModelEditDataResultStates.ReturnFactExpenditureState AddFactExpenditure(string expenditureName, string factExpenditureCategory, double sum, DateTime date, string cardName);
        public bool AddFactIncome(string incomeName, string factIncomeCategory, double sum, DateTime date, string cardName);
        public bool AddPlanExpenditure(string expenditureCategory, double sum, DateTime beginDate, DateTime endDate, string planExpenditureImagePath);
        public bool AddPlanIncome(string incomeCategory, double sum, DateTime beginDate, DateTime endDate, string planIncomeImagePath);
        // return list of class of income, expenditure
    }
}
