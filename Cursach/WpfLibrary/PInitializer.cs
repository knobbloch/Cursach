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
    public class PInitializer
    {
        static IAddExpensePageView addExpensee;
        static IAddExpenseCategoryPageView addExpenseCategoryy;
        static IAddIncomePageView addIncomee;
        static IAddIncomeCategoryPageView addIncomeCategoryy;
        static IModel modell;

        public PInitializer(IAnalysisPageView analysis, IDatedPageView date, IInterPageView inter, IPlanPageView plan,
                            IAddBankAccountPageView addCard, IAddExpenseCategoryPageView addExpenseCategory,
                            IAddExpensePageView addExpensePage, IAddIncomeCategoryPageView addIncomeCategory, IAddIncomePageView addIncomePage)
        {
            DbWorker model = new DbWorker("C:/other/hse/prog/курсовой/UserData.db");
            PDate dateCreating = new PDate(date);
            PAnalysis analysisCreate = new PAnalysis(analysis, model);
            PCard cardCreate = new PCard(addCard, model);
            PInterPage interPage = new PInterPage(inter, model, addCard);
            PPlan planPage = new PPlan(plan, model);
            modell = model;
            addExpensee = addExpensePage;
            addExpenseCategoryy = addExpenseCategory;
            addIncomee = addIncomePage;
            addIncomeCategoryy = addIncomeCategory;
        }

        public static void AddExpense()
        {
            PAddExpense addExpense = new PAddExpense(addExpensee, modell);
        }

        public static void AddExpenseCategory()
        {
            PAddPlanExpense addExpense = new PAddPlanExpense(addExpenseCategoryy, modell);
        }

        public static void AddIncome()
        {
            PAddIncome addIncome = new PAddIncome(addIncomee, modell);
        }

        public static void AddIncomeCategory()
        {
            PAddPlanIncome addIncome = new PAddPlanIncome(addIncomeCategoryy, modell);
        }
    }
}
