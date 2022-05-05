using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ApplicationProject.Views;
using ApplicationProject.Views.DatedPageView;
using System.Globalization;

namespace ApplicationProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewPresenter
    {
        public IBaseView PresentedView { get; protected set; }
        public Overlay Overlay { get; }

        public MainWindow()
        {
            InitializeComponent();
            Overlay = new Overlay(OverlayLayer);

            UserControls.InterPageView.InterPageView view = new()
            {
                AnalysisButtonNameKey = "Анализ",
                PlanButtonNameKey = "План",
                AccountName = "Аккаунт"
            };
            _ = Present(view);
            UserControls.DatedPageView.DatedPageView datedView = new()
            {
                PageNameTextKey = "Анализ"
            };
            datedView.DateRangeTypes.Add(new DateRangeType { DisplayName = "Месяц", Type = DateRangeType.RangeType.MONTH });
            datedView.DateRangeTypes.Add(new DateRangeType { DisplayName = "Год", Type = DateRangeType.RangeType.YEAR });
            _ = view.PageViewPresenter.Present(datedView);
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
            analysisPage.ExpensesChartItems.Add(new Views.AnalysisPageView.AnalysisPageChartExpenseEntry
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
            analysisPage.IncomeChartItems.Add(new Views.AnalysisPageView.AnalysisPageChartIncomeEntry
            {
                PeriodTitle = "Day -1",
                Value = 25
            });
            analysisPage.IncomeChartItems.Add(new Views.AnalysisPageView.AnalysisPageChartIncomeEntry
            {
                PeriodTitle = "Day -1",
                Value = 55
            });
            _ = datedView.PageViewPresenter.Present(analysisPage);
        }

        public void OnCultureChanged(CultureInfo culture)
        {
            PresentedView?.OnCultureChanged(culture);
        }

        public bool Present(IBaseView view)
        {
            if(view == null)
                throw new ArgumentNullException(nameof(view));
            else if(!view.IsPresentable || !(view is UserControl))
                return false;

            PresentedView?.Hide();
            if(PresentedView is ISupportOverlay overlay)
            {
                overlay.ClearOverlay();
                overlay.Overlay = null;
            }

            PresentedView = view;
            ActiveView.Content = view as UserControl;

            PresentedView?.Show();
            if(PresentedView is ISupportOverlay overlay2)
                overlay2.Overlay = Overlay;

            return true;
        }
    }
}
