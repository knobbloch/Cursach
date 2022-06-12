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
using ApplicationProjectViews;
using WpfMishaLibrary;

namespace WpfLibrary
{
    public class PrezenterInitializer
    {
        public PrezenterInitializer(IAnalysisPageView analysis, IDatedPageView date, IInterPageView inter, IPlanPageView plan,
                                    IAddBankAccountPageView addCard, IAddExpenseCategoryPageView addExpenseCategory,
                                    IAddExpensePageView addExpensePage, IAddIncomeCategoryPageView addIncomeCategory, IAddIncomePageView addIncome)
        {
            DbWorker model = new DbWorker("C:/other/hse/prog/курсовой/UserData.db");
            Date dateCreating = new Date(date);
            Analysis analysisCreate = new Analysis(analysis);
            Card cardCreate = new Card(addCard, model);

        }
    }
}
