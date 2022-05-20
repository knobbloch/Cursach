using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using ApplicationProject.Views;
using ApplicationProject.Views.AnalysisPageView;

using ApplicationProject.UserControls.AnalysisPageView;

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

            IAnalysisPageView aview = new AnalysisPageView();
            aview.AccountName = "Temp";
            aview.BankAccounts.Add(new BankAccountInfo("Test", "15", "RUB"));
            aview.DateRangeTypes.Add(new Views.DatedPageView.DateRangeType()
            {
                Type = Views.DatedPageView.DateRangeType.RangeType.MONTH
            });
            aview.ExpenesItems.Add(new AnalysisPageExpenseEntry()
            {
                Title = "Автобус",
                Value = 33,
                CurrencyIdentifier = "RUB"
            });
            aview.ExpensesDays.Add(new AnalysisPageExpenseDayEntry()
            {
                PeriodTitle = "Вчера",
                Value = 33
            });
            aview.IncomeDays.Add(new AnalysisPageIncomeDayEntry()
            {
                PeriodTitle = "Сегодня",
                Value = 2000
            });
            aview.IncomeItems.Add(new AnalysisPageIncomeEntry()
            {
                Title = "Стипендия",
                Value = 2000,
                CurrencyIdentifier = "RUB"
            });
            aview.SelectedRangeType = Views.DatedPageView.DateRangeType.RangeType.MONTH;

            _ = viewRoot.Present(aview);
        }
    }
}
