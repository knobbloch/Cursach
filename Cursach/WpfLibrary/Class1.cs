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

namespace WpfLibrary
{
    public class Class1
    {
        public Class1(IAnalysisPageView analysis, IDatedPageView date, IInterPageView inter, IPlanPageView plan)
        {
            Date dateCreating = new Date(date);
            //Analysis analysisCreate = new Analysis(IModel model);
        }
    }
}
