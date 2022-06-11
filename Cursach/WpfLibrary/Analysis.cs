using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.AnalysisPageView;
using WpfMishaLibrary;

namespace WpfLibrary
{
    class Analysis
    {
        public Analysis(IAnalysisPageView analysis)
        {
            analysis.AddExpenseAction += ClickButtonAddExcpense;
            analysis.AddExpenseCategoryAction += ClickButtonAddExcpenseCategoryAction;
            analysis.AddIncomeAction += ClickButtonAddIncome;
            analysis.AddIncomeCategoryAction += ClickButtonAddExcpenseCategoryIncome;
            analysis.ModeChanged += RegimeChange;
            //analysis.IncomeEntrySelected += RecordIncome;
            //analysis.ExpenseEntrySelected += RecordExcpense;
            //analysis.CurrentMode = IAnalysisPageView.AnalysisPageMode.Expenses;
            AnalysisPageExpenseEntry obj = new AnalysisPageExpenseEntry() { Value = 55, Date = new DateTime(2222, 11, 3), Title = "f" };
            analysis.ExpensesItems.Add(obj);
        }

        public void ClickButtonAddExcpense(object source, EventArgs a)//добавить расходы
        {
        }
        public void ClickButtonAddExcpenseCategoryAction(object source, EventArgs a)//добавить категорию расхода
        {
        }
        public void ClickButtonAddIncome(object source, EventArgs a)//добавить доход
        {
        }
        public void ClickButtonAddExcpenseCategoryIncome(object source, EventArgs a)//добавить категорию дохода
        {
        }

        public void RegimeChange(object source, AnalysisPageModeSelectedEventArgs a)//меняется режим
        {
            ((IAnalysisPageView)source).DispatchUpdate(view =>
            {
                IAnalysisPageView analysisiView = (IAnalysisPageView)view;
                if (analysisiView.CurrentMode == IAnalysisPageView.AnalysisPageMode.Income)
                    analysisiView.CurrentMode = IAnalysisPageView.AnalysisPageMode.Expenses;
                else
                    analysisiView.CurrentMode = IAnalysisPageView.AnalysisPageMode.Income;
            });
        }

        /*public void RecordIncome(object source, AnalysisPageIncomeEntrySelectedEventArgs a)//выбор записи расхода
        {  
        }

        public void RecordExcpense(object source, AnalysisPageExpenseEntrySelectedEventArgs a)//выбор записи доходав
        {  
        }*/

        private void ShowExcpenses()//почти все
        {
            /*
            TotalExpenses
            ExpensesItems AnalysisPageExpenseEntry                      
                                Value = 0;                              CardName
                                CurrencyIdentifier = "";                Date        
                                Title = "";                             DateDateTime
                                Category = new CategoryDescriptor();    ExpenditureName
                                Date = default;                         FactExpenditureCategory
                                                                        Sum

            ExpensesDays   AnalysisPageExpenseDayEntry (value, periodTitle)
            */
        }

        private void ShowIncomes()//почти все
        {
            /*
            TotalIncome
            IncomeItems AnalysisPageIncomeEntry     Value = 0;
                                                    CurrencyIdentifier = "";
                                                    Title = "";
                                                    Category = new CategoryDescriptor();
                                                    Date = default;

            IncomeDays AnalysisPageIncomeDayEntry
            */
        }

        /*public void ListFromFactExpenditureToListAnalysisPageExpenseEntry(ICollection<AnalysisPageExpenseEntry> analysis)
        {
            for (int i = 0; i<source.Count; i++)
            {
                AnalysisPageExpenseEntry obj = new AnalysisPageExpenseEntry() { Value = 55, Date = new DateTime(2222, 11, 3), Title = "f" };
                analysis.Add(obj);
            }
            return received;
        }

        public void GraphFromFactExpenditureToListAnalysisPageExpenseDayEntry ()
        {
            for (int i = 0; i < source.Count; i++)
            {
                AnalysisPageExpenseDayEntry obj = new AnalysisPageExpenseDayEntry() { Value = 55, Date = new DateTime(2222, 11, 3), Title = "f" };
                analysis.ExpensesItems.Add(obj);
            }
        }*/

        /*public void ListFromPlanExpenditureToListAnalysisPageIncomeEntry()
        {
            for (int i = 0; i<source.Count; i++)
            {
                AnalysisPageExpenseEntry obj = new AnalysisPageExpenseEntry() { Value = 55, Date = new DateTime(2222, 11, 3), Title = "f" };
                analysis.ExpensesItems.Add(obj);
            }
        }

        public void GraphFromPlanExpenditureToListAnalysisPageIncomeDayEntry ()
        {
            analysis.C
            for (int i = 0; i < source.Count; i++)
            {
                AnalysisPageIncomeDayEntry obj = new AnalysisPageIncomeDayEntry() { Value = 55, Date = new DateTime(2222, 11, 3), Title = "f" };
                received.ExpensesItems.Add(obj);
            }
        }*/
    }
}
