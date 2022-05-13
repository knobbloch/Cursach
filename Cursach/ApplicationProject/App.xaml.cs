using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using ApplicationProject.Views;
using ApplicationProject.Views.DatedPageView;

namespace ApplicationProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            IViewPresenter viewRoot = (IViewPresenter)(MainWindow = new MainWindow());
            MainWindow.Show();

            UserControls.InterPageView.InterPageView view = new()
            {
                AnalysisButtonNameKey = "Анализ",
                PlanButtonNameKey = "План",
                AccountName = "Аккаунт"
            };
            _ = viewRoot.Present(view);
            UserControls.DatedPageView.DatedPageView datedView = new()
            {
                PageNameTextKey = "Анализ"
            };
            datedView.DateRangeTypes.Add(new DateRangeType { DisplayName = "Месяц", Type = DateRangeType.RangeType.MONTH });
            datedView.DateRangeTypes.Add(new DateRangeType { DisplayName = "Год", Type = DateRangeType.RangeType.YEAR });
            _ = view.PageViewPresenter.Present(datedView);
            /*
            UserControls.AnalysisPageView.AnalysisPageView analysisPage = new()
            {
                ExpensesTableNameHeaderKey = "Категория",
                ExpensesTableValueHeaderKey = "Расход",
                IncomeTableNameHeaderKey = "Источник",
                IncomeTableValueHeaderKey = "Доход",
                ExpensesTabNameKey = "Расходы",
                IncomeTabNameKey = "Доходы",
                AddExpenseTextKey = "Добавить расход",
                AddExpenseCategoryTextKey = "Добавить категорию расходов",
                CreateExpensesReportTextKey = "Создать файл-отчёт по расходам",
                AddIncomeTextKey = "Добавить доход",
                CreateIncomeReportTextKey = "Создать файл-отчёт по доходам"
            };
            analysisPage.ExpenesItems.Add(new Views.AnalysisPageView.AnalysisPageExpenseEntry
            {
                CurrencyIdentifier = "RUB",
                Value = 100,
                Title = "Тест",
                ImagePath = "C:\\HSE\\Курсач\\Cursach\\ApplicationProject\\Resources\\profilePicture.png"
            });
            analysisPage.ExpensesChartItems.Add(new Views.AnalysisPageView.AnalysisPageExpenseChartEntry
            {
                PeriodTitle = "Day 1",
                Value = 25
            });
            analysisPage.IncomeItems.Add(new Views.AnalysisPageView.AnalysisPageIncomeEntry
            {
                Title = "Стипа",
                CurrencyIdentifier = "RUB",
                Value = 100500
            });
            analysisPage.IncomeChartItems.Add(new Views.AnalysisPageView.AnalysisPageIncomeChartEntry
            {
                PeriodTitle = "Day -1",
                Value = 25
            });
            analysisPage.IncomeChartItems.Add(new Views.AnalysisPageView.AnalysisPageIncomeChartEntry
            {
                PeriodTitle = "Day -1",
                Value = 55
            });
            _ = datedView.PageViewPresenter.Present(analysisPage);*/
            UserControls.PlanPageView.PlanPageView planPage = new UserControls.PlanPageView.PlanPageView()
            {
                ExpensesTableNameHeaderKey = "Категория",
                ExpensesTableRealValueHeaderKey = "Факт",
                ExpensesTablePlannedValueHeaderKey = "План",
                IncomeTableNameHeaderKey = "Зачисления",
                IncomeTableRealValueHeaderKey = "Факт",
                IncomeTablePlannedValueHeaderKey = "План",
                ExpensesTabNameKey = "Расходы",
                IncomeTabNameKey = "Доходы",
                AddExpenseCategoryTextKey = "Добавить категорию расходов",
                CreateExpensesReportTextKey = "Создать файл-отчёт по расходам",
                CreateIncomeReportTextKey = "Создать файл-отчёт по доходам"
            };
            planPage.ExpenesItems.Add(new Views.PlanPageView.PlanPageExpenseEntry
            {
                CurrencyIdentifier = "RUB",
                PlannedValue = 100,
                RealValue = 250,
                Title = "Тест",
                ImagePath = "C:\\HSE\\Курсач\\Cursach\\ApplicationProject\\Resources\\profilePicture.png"
            });
            planPage.ExpensesChartItems.Add(new Views.PlanPageView.PlanPageExpenseChartEntry
            {
                ImagePath = "C:\\HSE\\Курсач\\Cursach\\ApplicationProject\\Resources\\profilePicture.png",
                Value = 25
            });
            planPage.IncomeItems.Add(new Views.PlanPageView.PlanPageIncomeEntry
            {
                Title = "Стипа",
                CurrencyIdentifier = "RUB",
                PlannedValue = 100500,
                RealValue = 2
            });
            planPage.IncomeChartItems.Add(new Views.PlanPageView.PlanPageIncomeChartEntry
            {
                Title = "Day -1",
                Value = 25
            });
            planPage.IncomeChartItems.Add(new Views.PlanPageView.PlanPageIncomeChartEntry
            {
                Title = "Day -1",
                Value = 55
            });
            _ = datedView.PageViewPresenter.Present(planPage);
        }
    }
}
