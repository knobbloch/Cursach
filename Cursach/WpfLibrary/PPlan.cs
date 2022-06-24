using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.PlanPageView;
using WpfMishaLibrary;

namespace WpfLibrary
{
    class PPlan
    {
        static IModel m;
        static IPlanPageView planPage;
        public PPlan(IPlanPageView plan, WpfMishaLibrary.IModel model)
        {
            planPage = plan;
            plan.AddExpenseCategoryAction += AddExpenseCategoryClicked;
            plan.AddIncomeCategoryAction += AddIncomeCategoryClicked;
            plan.CurrentMode = IPlanPageView.PlanPageMode.Expenses;
            m = model;
            Update();
        }


        public static void Update()
        {
            ShowExcpenses();
            ShowIncomes();
        }


        public void RegimeChange(object source, PlanPageModeSelectedEventArgs a)//меняется режим
        {
            ((IPlanPageView)source).DispatchUpdate(view =>
            {
                IPlanPageView planView = (IPlanPageView)view;
                if (planView.CurrentMode == IPlanPageView.PlanPageMode.Income)
                    planView.CurrentMode = IPlanPageView.PlanPageMode.Expenses;
                else
                    planView.CurrentMode = IPlanPageView.PlanPageMode.Income;
            });
        }

        public static void ShowExcpenses()
        {
            ((IPlanPageView)planPage).DispatchUpdate(view =>
            {
                IPlanPageView planiView = (IPlanPageView)view;
                List<WpfMishaLibrary.VisibleEntities.PlanExpenditureVisible> list = m.GetPlanExpendituresDiapason(PDate.DateBounds.Start, PDate.DateBounds.End);
                List<WpfMishaLibrary.VisibleEntities.FactExpenditureVisible> listFact = m.GetFactExpendituresDiapason(PDate.DateBounds.Start, PDate.DateBounds.End);
                planiView.ExpensesItems.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    double real = 0;
                    try { real = listFact[i].Sum; } catch { };
                    planiView.ExpensesItems.Add(new PlanPageExpenseEntry()
                    { Title = list[i].ExpenditureCategory, PlannedValue = list[i].Sum, RealValue = real, ImagePath = listFact[i].CategoryImagePath});
                }
            });
        }

        public static void ShowIncomes()
        {
            ((IPlanPageView)planPage).DispatchUpdate(view =>
            {
                IPlanPageView planView = (IPlanPageView)view;
                List<WpfMishaLibrary.VisibleEntities.PlanIncomeVisible> list = m.GetPlanIncomesDiapason(PDate.DateBounds.Start, PDate.DateBounds.End);
                List<WpfMishaLibrary.VisibleEntities.FactIncomeVisible> listFact = m.GetFactIncomesDiapason(PDate.DateBounds.Start, PDate.DateBounds.End);
                planView.IncomeItems.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    double real = 0;
                    try {real = listFact[i].Sum;} catch { };
                    planView.IncomeItems.Add(new PlanPageIncomeEntry()
                    { Title = list[i].IncomeCategory, PlannedValue = list[i].Sum, RealValue = real, ImagePath = listFact[i].CategoryImagePath });
                }
            });
        }

        public void AddExpenseClicked(object source, EventArgs a)
        {
            PInitializer.AddExpense();
        }

        public void AddExpenseCategoryClicked(object source, EventArgs a)
        {
            PInitializer.AddExpenseCategory();
        }


        public void AddIncomeClicked(object source, EventArgs a)
        {
            PInitializer.AddIncome();
        }

        public void AddIncomeCategoryClicked(object source, EventArgs a)
        {
            PInitializer.AddIncomeCategory();
        }
    }
}

