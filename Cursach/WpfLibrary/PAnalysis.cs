using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.AnalysisPageView;
using WpfMishaLibrary;

namespace WpfLibrary
{
    class PAnalysis
    {
        static IModel m;
        public static IAnalysisPageView analysisPage;
        public PAnalysis(IAnalysisPageView analysis, WpfMishaLibrary.IModel model)
        {
            analysisPage = analysis;
            analysis.ModeChanged += RegimeChange;

            analysis.AddExpenseAction += AddExpenseClicked;
            analysis.AddExpenseCategoryAction += AddExpenseCategoryClicked;

            analysis.AddIncomeAction += AddIncomeClicked;
            analysis.AddIncomeCategoryAction += AddIncomeCategoryClicked;

            m = model;
            Update();
        }

        public static void Update()
        {
            ShowExcpenses();
            GraphExpense();
            ShowIncomes();
            GraphIncome();
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

        public static void ShowExcpenses()
        {
            ((IAnalysisPageView)analysisPage).DispatchUpdate(view =>
            {
                IAnalysisPageView analysisiView = (IAnalysisPageView)view;
                List<WpfMishaLibrary.VisibleEntities.FactExpenditureVisible> list = m.GetFactExpendituresDiapason(PDate.DateBounds.Start, PDate.DateBounds.End);
                analysisiView.ExpensesItems.Clear();
                decimal sum = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    //string a = list[i].FactExpenditureCategory ;
                    analysisiView.ExpensesItems.Add(new AnalysisPageExpenseEntry()
                    { Title = list[i].ExpenditureName, Category = new ApplicationProjectViews.CategoryDescriptor() { DisplayName = list[i].FactExpenditureCategory, ImagePath = list[i].CategoryImagePath }, Date = list[i].DateDateTime.Date, Value = list[i].Sum });
                    sum += Convert.ToDecimal(list[i].Sum);
                }
                analysisiView.TotalExpenses = sum;
            });
        }

        public static void ShowIncomes()
        {
            ((IAnalysisPageView)analysisPage).DispatchUpdate(view =>
            {
                IAnalysisPageView analysisiView = (IAnalysisPageView)view;
                List<WpfMishaLibrary.VisibleEntities.FactIncomeVisible> list = m.GetFactIncomesDiapason(PDate.DateBounds.Start, PDate.DateBounds.End);
                analysisiView.IncomeItems.Clear();
                decimal sum = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    analysisiView.IncomeItems.Add(new AnalysisPageIncomeEntry()
                    { Title = list[i].FactIncomeName, Category = new ApplicationProjectViews.CategoryDescriptor() { DisplayName = list[i].FactIncomeCategory, ImagePath = list[i].CategoryImagePath }, Date = list[i].DateDateTime.Date, Value = list[i].Sum });
                    sum += Convert.ToDecimal(list[i].Sum);
                }
                analysisiView.TotalIncome = sum;
            });
        }

        public static void GraphIncome()
        {
            ((IAnalysisPageView)analysisPage).DispatchUpdate(view =>
            {
                IAnalysisPageView analysis = (IAnalysisPageView)view;
                analysis.IncomeDays.Clear();

                if (PDate.dateType)//год
                {
                    for (int i = 0; i < 12; i++)
                    {
                        double sum = 0;
                        DateTime tmp = new DateTime(2010, PDate.DateBounds.Start.Month, 1);
                        tmp = tmp.AddMonths(i);
                        DateTime tmp2 = tmp.AddMonths(1).AddDays(-1);
                        List<WpfMishaLibrary.VisibleEntities.FactIncomeVisible> list1 = m.GetFactIncomesDiapason(new DateTime(PDate.DateBounds.Start.Year, tmp.Month, 1), new DateTime(PDate.DateBounds.Start.Year, tmp.Month, tmp2.Day));
                        for (int j = 0; j < list1.Count; j++)
                            sum += list1[j].Sum;
                        analysis.IncomeDays.Add(new AnalysisPageIncomeDayEntry() { PeriodTitle = (i + 1).ToString(), Value = sum });
                    }
                }
                else
                {
                    DateTime tmp = new DateTime(1, PDate.DateBounds.Start.Month + 1, 1);
                    tmp = tmp.AddDays(-1);
                    for (int i = 0; i < tmp.Day; i++)
                    {
                        double sum = 0;
                        List<WpfMishaLibrary.VisibleEntities.FactIncomeVisible> list1 = m.GetFactIncomesDiapason(new DateTime(PDate.DateBounds.Start.Year, PDate.DateBounds.Start.Month, i + 1), new DateTime(PDate.DateBounds.Start.Year, PDate.DateBounds.Start.Month, i + 1));
                        for (int j = 0; j < list1.Count; j++)
                            sum += list1[j].Sum;
                        analysis.IncomeDays.Add(new AnalysisPageIncomeDayEntry() { PeriodTitle = (i + 1).ToString(), Value = sum });
                    }
                }
            });
        }

        public static void GraphExpense()
        {
            ((IAnalysisPageView)analysisPage).DispatchUpdate(view =>
            {
                IAnalysisPageView analysis = (IAnalysisPageView)view;
                analysis.ExpensesDays.Clear();

                if (PDate.dateType)//год
                {
                    for (int i = 0; i < 12; i++)
                    {
                        double sum = 0;
                        DateTime tmp = new DateTime(2010, PDate.DateBounds.Start.Month, 1);
                        tmp = tmp.AddMonths(i);
                        DateTime tmp2 = tmp.AddMonths(1).AddDays(-1);
                        List<WpfMishaLibrary.VisibleEntities.FactExpenditureVisible> list1 = m.GetFactExpendituresDiapason(new DateTime(PDate.DateBounds.Start.Year, tmp.Month, 1), new DateTime(PDate.DateBounds.Start.Year, tmp.Month, tmp2.Day));
                        for (int j = 0; j < list1.Count; j++)
                            sum += list1[j].Sum;
                        analysis.ExpensesDays.Add(new AnalysisPageExpenseDayEntry() { PeriodTitle = (i + 1).ToString(), Value = sum });
                    }
                }
                else
                {
                    DateTime tmp = new DateTime(1, PDate.DateBounds.Start.Month + 1, 1);
                    tmp = tmp.AddDays(-1);
                    for (int i = 0; i < tmp.Day; i++)
                    {
                        double sum = 0;
                        List<WpfMishaLibrary.VisibleEntities.FactExpenditureVisible> list1 = m.GetFactExpendituresDiapason(new DateTime(PDate.DateBounds.Start.Year, PDate.DateBounds.Start.Month, i+1), new DateTime(PDate.DateBounds.Start.Year, PDate.DateBounds.Start.Month, i+1));
                        for (int j = 0; j < list1.Count; j++)
                            sum += list1[j].Sum;
                        analysis.ExpensesDays.Add(new AnalysisPageExpenseDayEntry() { PeriodTitle = (i+1).ToString(), Value = sum });
                    }
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
