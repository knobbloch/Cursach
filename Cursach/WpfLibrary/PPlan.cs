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
                IPlanPageView planView = (IPlanPageView)view;
                List<WpfMishaLibrary.VisibleEntities.PlanExpenditureVisible> list = m.GetPlanExpendituresDiapason(PDate.DateBounds.Start, PDate.DateBounds.End);
                List<WpfMishaLibrary.VisibleEntities.FactExpenditureVisible> listFact = m.GetFactExpendituresDiapason(PDate.DateBounds.Start, PDate.DateBounds.End);
                planView.ExpensesItems.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    double real = 0;
                    for (int j = 0; j < listFact.Count; j++)
                    {
                        if (listFact[j].FactExpenditureCategory == list[i].ExpenditureCategory)
                            real += listFact[j].Sum;
                    }
                    planView.ExpensesItems.Add(new PlanPageExpenseEntry()
                    { Title = list[i].ExpenditureCategory, PlannedValue = list[i].Sum, RealValue = real, ImagePath = list[i].PlanExpenditureImagePath});
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
                    for (int j = 0; j < listFact.Count; j++)
                    {
                        if (listFact[j].FactIncomeCategory == list[i].IncomeCategory)
                            real += listFact[j].Sum;
                    }
                    planView.IncomeItems.Add(new PlanPageIncomeEntry()
                    { Title = list[i].IncomeCategory, PlannedValue = list[i].Sum, RealValue = real, ImagePath = list[i].PlanIncomeImagePath });
                }
            });
        }
    }
}

