using System;
using ApplicationProjectViews.AddBankAccountPageView;
using ApplicationProjectViews.AddExpenseCategoryPageView;
using ApplicationProjectViews.AddExpensePageView;
using ApplicationProjectViews.AddIncomeCategoryPageView;
using ApplicationProjectViews.AddIncomePageView;
using ApplicationProjectViews.AnalysisPageView;
using ApplicationProjectViews.DatedPageView;
using ApplicationProjectViews.InterPageView;
using ApplicationProjectViews.PlanPageView;
using WpfMishaLibrary;
using System.IO;

namespace WpfLibrary
{
    public class PInitializer
    {
        public PInitializer(IAnalysisPageView analysis, IDatedPageView date, IInterPageView inter, IPlanPageView plan,
                            IAddBankAccountPageView addCard, IAddExpenseCategoryPageView addExpenseCategory,
                            IAddExpensePageView addExpensePage, IAddIncomeCategoryPageView addIncomeCategory, IAddIncomePageView addIncomePage)
        {
            //MessageBox.Show(s, "111", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes);
            //DbWorker model = new DbWorker("C:/other/hse/prog/курсовой/UserData.db");
            //DbWorker model = new DbWorker("./UserData.db");
            DbWorker model = new DbWorker(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\UserData.db");
            PDate dateCreating = new PDate(date);
            PAnalysis analysisCreate = new PAnalysis(analysis, model);
            PCard cardCreate = new PCard(addCard, model);
            PInterPage interPage = new PInterPage(inter, model, addCard);
            PPlan planPage = new PPlan(plan, model);
            PAddExpense addExpense = new PAddExpense(addExpensePage, model);
            PAddPlanExpense addExpenseCategory1 = new PAddPlanExpense(addExpenseCategory, model);
            PAddIncome addIncome = new PAddIncome(addIncomePage, model);
            PAddPlanIncome addIncome1 = new PAddPlanIncome(addIncomeCategory, model);
        }
    }
}
