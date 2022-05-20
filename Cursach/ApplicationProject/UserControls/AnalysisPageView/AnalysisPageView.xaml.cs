using System;
using System.Globalization;
using System.ComponentModel;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using ApplicationProject.Views;
using ApplicationProject.Views.InterPageView;
using ApplicationProject.Views.DatedPageView;
using ApplicationProject.Views.AnalysisPageView;

namespace ApplicationProject.UserControls.AnalysisPageView
{
    /// <summary>
    /// Interaction logic for AnalysisPageView.xaml
    /// </summary>
    public partial class AnalysisPageView : UserControl, IAnalysisPageView, ISupportOverlay, INotifyPropertyChanged
    {
        protected static readonly Point CalendarOffset = new(0, 0);
        protected const string ExpensesTabNameKey = "PAGE_ANALYSIS_TAB_EXPENSES_NAME";
        protected const string IncomeTabNameKey = "PAGE_ANALYSIS_TAB_INCOME_NAME";
        protected const string ExpensesTableNameHeaderKey = "PAGE_ANALYSIS_TAB_EXPENSES_TABLE_HEADER_NAME";
        protected const string ExpensesTableValueHeaderKey = "PAGE_ANALYSIS_TAB_EXPENSES_TABLE_HEADER_VALUE";
        protected const string IncomeTableNameHeaderKey = "PAGE_ANALYSIS_TAB_EXPENSES_TABLE_HEADER_NAME";
        protected const string IncomeTableValueHeaderKey = "PAGE_ANALYSIS_TAB_EXPENSES_TABLE_HEADER_VALUE";
        protected const string AddExpenseTextKey = "PAGE_ANALYSIS_TAB_EXPENSES_BUTTON_ADD";
        protected const string AddExpenseCategoryTextKey = "PAGE_ANALYSIS_TAB_EXPENSES_BUTTON_ADDCATEGORY";
        protected const string CreateExpensesReportTextKey = "PAGE_ANALYSIS_TAB_EXPENSES_BUTTON_CREATEREPORT";
        protected const string AddIncomeTextKey = "PAGE_ANALYSIS_TAB_INCOME_BUTTON_ADD";
        protected const string CreateIncomeReportTextKey = "PAGE_ANALYSIS_TAB_INCOME_CREATEREPORT";
        protected const string PageNameTextKey = "PAGE_ANALYSIS_NAME";
        protected const string AnalysisButtonNameKey = "PAGE_ANALYSIS_BUTTON_ANALYSIS";
        protected const string PlanButtonNameKey = "PAGE_ANALYSIS_BUTTON_PLAN";

        public AnalysisPageView()
        {
            InitializeComponent();

            BankAccountsDisplayer.ItemsSource = BankAccounts = new ObservableCollection<BankAccountInfo>();

            DateRangeTypes = new ObservableCollection<DateRangeType>();
            ((ObservableCollection<DateRangeType>)DateRangeTypes).CollectionChanged += AnalysisPage_DateRangeTypesChanged;
            DateRangeTypeSelector.ItemsSource = DateRangeTypes;

            DateRangeSelectorRoot = new Viewbox
            {
                Child = new RangeSelectorCalendar()
            };

            DateRangeSelectorCalendar = (RangeSelectorCalendar)DateRangeSelectorRoot.Child;
            DateRangeSelectorCalendar.SelectionTarget = RangeSelectorCalendar.RangeSelectorCalendarMode.Month;
            DateRangeSelectorCalendar.SelectionChanged += DaterRangeSelector_SelectionChanged;
            CurrentCulture = null;

            IncomeBarChart.BarsSource = IncomeDays = new ObservableCollection<AnalysisPageIncomeDayEntry>();
            ExpensesBarChart.BarsSource = ExpensesDays = new ObservableCollection<AnalysisPageExpenseDayEntry>();
            IncomeList.ItemsSource = IncomeItems = new ObservableCollection<AnalysisPageIncomeEntry>();
            ExpensesList.ItemsSource = ExpenesItems = new ObservableCollection<AnalysisPageExpenseEntry>();
        }

        protected Viewbox DateRangeSelectorRoot { get; }
        protected RangeSelectorCalendar DateRangeSelectorCalendar { get; }

        protected CultureInfo CurrentCulture
        {
            get => m_CurrentCulture;
            set
            {
                m_CurrentCulture = value ?? System.Threading.Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture ?? CultureInfo.InvariantCulture;
                DateRangeSelectorCalendar.CurrentCulture = m_CurrentCulture;

                RefreshLocalization();
            }
        }
        private CultureInfo m_CurrentCulture;

        #region IBaseView
        public void Show() { }
        public void Hide() { }
        public bool IsPresentable => ExpensesTableNameHeader.Length > 0 &&
                                     ExpensesTableValueHeader.Length > 0 &&
                                     IncomeTableNameHeader.Length > 0 &&
                                     IncomeTableValueHeader.Length > 0 &&
                                     ExpensesTabName.Length > 0 &&
                                     IncomeTabName.Length > 0 &&
                                     AddExpenseText.Length > 0 &&
                                     AddExpenseCategoryText.Length > 0 &&
                                     CreateExpensesReportText.Length > 0 &&
                                     AddIncomeText.Length > 0 &&
                                     CreateIncomeReportText.Length > 0 &&
                                     DateRangeTypes.Count > 0 &&
                                     PageNameText.Length > 0 &&
                                     AnalysisButtonName.Length > 0 &&
                                     PlanButtonName.Length > 0 &&
                                     AccountName.Length > 0;

        public void OnCultureChanged(CultureInfo newCulture)
        {
            CurrentCulture = newCulture;

            foreach (DateRangeType type in DateRangeTypes)
                type.OnCultureChanged(newCulture);
        }

        public void DispatchUpdate(ViewUpdate action)
        {
            Dispatcher.Invoke(() => action(this));
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region IInterPageView
        public string AnalysisButtonName => AnalysisButtonNameKey;
        public string AnalysisButtonSymbol => AnalysisButtonName.Substring(0, 1);

        public string PlanButtonName => PlanButtonNameKey;
        public string PlanButtonSymbol => PlanButtonName.Substring(0, 1);

        private string m_AccountName;
        public string AccountName
        {
            get => m_AccountName;
            set
            {
                m_AccountName = value ?? "";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountName)));
            }
        }

        public event CategorySelectedEventHandler CategorySelectedAction;
        public event EventHandler ProfileSelectedAction;
        public event BankAccountSelectedSelectedEventHandler BankAccountSelected;

        public IList<BankAccountInfo> BankAccounts { get; }
        #endregion

        #region ISupportOverlay
        private Overlay m_Overlay;
        public Overlay Overlay
        {
            get => m_Overlay;
            set
            {
                m_Overlay = value ?? throw new ArgumentNullException(nameof(value));
                m_Overlay.BackgroundClick += Overlay_Click;
            }
        }

        public void ClearOverlay()
        {
            Overlay.BackgroundClick -= Overlay_Click;
        }
        #endregion

        #region IDatedPageView
        public string DateRangeText => ConvertToDateRangeDisplay(DisplayedDateRange);
        public string PageNameText => PageNameTextKey;

        public event DateRangeTypeSelectedEventHandler DateRangeTypeSelected;
        public event DateRangeSelectedEventHandler DateRangeSelected;
        public event EventHandler NextDateRangeSelected;
        public event EventHandler PreviousDateRangeSelected;

        public DateRange DisplayedDateRange
        {
            get => m_DisplayedDateRange;
            set
            {
                m_DisplayedDateRange = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateRangeText)));
            }
        }
        private DateRange m_DisplayedDateRange;

        public ICollection<DateRangeType> DateRangeTypes { get; }

        public DateRange? DateRangeBounds
        {
            get => new(DateRangeSelectorCalendar.LowerBoundary.Value, DateRangeSelectorCalendar.UpperBoundary.Value);
            set
            {
                DateRangeSelectorCalendar.LowerBoundary = value.HasValue ? value.Value.Start : null;
                DateRangeSelectorCalendar.UpperBoundary = value.HasValue ? value.Value.End : null;
            }
        }

        public DateRangeType.RangeType SelectedRangeType
        {
            get => m_SelectedRangeType;
            set
            {
                m_SelectedRangeType = value;
                DateRangeSelectorCalendar.SelectionTarget = m_SelectedRangeType switch
                {
                    DateRangeType.RangeType.MONTH => RangeSelectorCalendar.RangeSelectorCalendarMode.Month,
                    DateRangeType.RangeType.YEAR => RangeSelectorCalendar.RangeSelectorCalendarMode.Year,
                    _ => throw new ArgumentOutOfRangeException(nameof(SelectedRangeType)),
                };
            }
        }
        private DateRangeType.RangeType m_SelectedRangeType;
        #endregion

        #region IAnalysisPageView
        public string ExpensesTabName => ExpensesTabNameKey;
        public string IncomeTabName => IncomeTabNameKey;
        public string ExpensesTableNameHeader => ExpensesTableNameHeaderKey;
        public string ExpensesTableValueHeader => ExpensesTableValueHeaderKey;
        public string IncomeTableNameHeader => IncomeTableNameHeaderKey;
        public string IncomeTableValueHeader => IncomeTableValueHeaderKey;
        public string AddExpenseText => AddExpenseTextKey;
        public string AddExpenseCategoryText => AddExpenseCategoryTextKey;
        public string CreateExpensesReportText => CreateExpensesReportTextKey;
        public string AddIncomeText => AddIncomeTextKey;
        public string CreateIncomeReportText => CreateIncomeReportTextKey;

        public event EventHandler AddExpenseAction;
        public event EventHandler AddExpenseCategoryAction;
        public event EventHandler CreateExpensesReportAction;
        public event EventHandler AddIncomeAction;
        public event EventHandler CreateIncomeReportAction;
        public event AnalysisPageTabSelectedEventHandler ModeChanged;
        public event AnalysisPageIncomeEntrySelectedEventHandler IncomeEntrySelected;
        public event AnalysisPageExpenseEntrySelectedEventHandler ExpenseEntrySelected;

        public IAnalysisPageView.AnalysisPageMode CurrentMode
        {
            get => m_CurrentMode;
            set
            {
                TabsControl.SelectedIndex = value switch
                {
                    IAnalysisPageView.AnalysisPageMode.Expenses => 0,
                    IAnalysisPageView.AnalysisPageMode.Income => 1,
                    _ => throw new ArgumentOutOfRangeException(nameof(CurrentMode))
                };

                m_CurrentMode = value;
            }
        }
        private IAnalysisPageView.AnalysisPageMode m_CurrentMode;
        public ICollection<AnalysisPageIncomeDayEntry> IncomeDays { get; }
        public ICollection<AnalysisPageExpenseDayEntry> ExpensesDays { get; }
        public ICollection<AnalysisPageIncomeEntry> IncomeItems { get; }
        public ICollection<AnalysisPageExpenseEntry> ExpenesItems { get; }
        #endregion

        #region Methods
        public void RefreshLocalization()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AnalysisButtonName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AnalysisButtonSymbol)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlanButtonName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlanButtonSymbol)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageNameText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateRangeText)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTabName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTabName)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTableNameHeader)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTableValueHeader)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTableNameHeader)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTableValueHeader)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddExpenseText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddExpenseCategoryText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreateExpensesReportText)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddIncomeText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreateIncomeReportText)));
        }

        protected string ConvertToDateRangeDisplay(DateRange range)
        {
            return m_SelectedRangeType switch
            {
                DateRangeType.RangeType.MONTH => range.Start.ToString("MMMM yyyy", CurrentCulture),
                DateRangeType.RangeType.YEAR => range.Start.ToString("yyyy", CurrentCulture),
                _ => "ERROR"
            };
        }
        #endregion

        #region Handled events
        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseAction?.Invoke(this, EventArgs.Empty);
        }

        private void AddExpenseCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseCategoryAction?.Invoke(this, EventArgs.Empty);
        }

        private void CreateExpensesReportButton_Click(object sender, RoutedEventArgs e)
        {
            CreateExpensesReportAction?.Invoke(this, EventArgs.Empty);
        }

        private void TabsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModeChanged?.Invoke(this, new AnalysisPageModeSelectedEventArgs(TabsControl.SelectedIndex switch
            {
                0 => IAnalysisPageView.AnalysisPageMode.Expenses,
                1 => IAnalysisPageView.AnalysisPageMode.Income,
                _ => throw new InvalidOperationException("Invalid tab was selected")
            }));
        }

        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            AddIncomeAction?.Invoke(this, EventArgs.Empty);
        }

        private void CreateIncomeReportButton_Click(object sender, RoutedEventArgs e)
        {
            CreateIncomeReportAction?.Invoke(this, EventArgs.Empty);
        }

        private void ExpensesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExpenseEntrySelected?.Invoke(this, new AnalysisPageExpenseEntrySelectedEventArgs((AnalysisPageExpenseEntry)ExpensesList.SelectedItem));
        }

        private void IncomeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IncomeEntrySelected?.Invoke(this, new AnalysisPageIncomeEntrySelectedEventArgs((AnalysisPageIncomeEntry)IncomeList.SelectedItem));
        }

        private void DateRangeSelector_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == DateRangeSelector)
            {
                DateRangeSelectorRoot.Height = DateRangeSelectorRoot.Width = DateRangeSelector.ActualWidth;
                Overlay.AddElement(DateRangeSelectorRoot);
                Overlay.MoveElement(DateRangeSelectorRoot, DateRangeSelector, new Point(CalendarOffset.X, CalendarOffset.Y + DateRangeSelector.ActualHeight));
                Overlay.Visible = DateRangeSelector.IsChecked ?? false;
                DateRangeSelectorRoot.Visibility = (DateRangeSelector.IsChecked ?? false) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void ButtonPreviousDateRange_Click(object sender, RoutedEventArgs e)
        {
            PreviousDateRangeSelected?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonNextDateRange_Click(object sender, RoutedEventArgs e)
        {
            NextDateRangeSelected?.Invoke(this, EventArgs.Empty);
        }

        private void Overlay_Click(object sender, EventArgs e)
        {
            Overlay.Visible = false;
            DateRangeSelector.IsChecked = false;
            Overlay.RemoveElement(DateRangeSelectorRoot);
        }

        private void CurrentPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Overlay.Visible)
            {
                DateRangeSelectorRoot.Height = DateRangeSelectorRoot.Width = DateRangeSelector.ActualWidth;
                Overlay.MoveElement(DateRangeSelectorRoot, DateRangeSelector, new Point(CalendarOffset.X, CalendarOffset.Y + DateRangeSelector.ActualHeight));
            }
        }

        private void DateRangeTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateRangeTypeSelected?.Invoke(this, new DateRangeTypeSelectedEventArgs((DateRangeType)DateRangeTypeSelector.SelectedItem));
        }

        private void AnalysisPage_DateRangeTypesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DateRangeTypes.Count == 1)
                DateRangeTypeSelector.SelectedIndex = 0;
        }

        private void DaterRangeSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (sender == DateRangeSelectorCalendar)
            {
                foreach (RangeSelectorCalendar.DateRange range in DateRangeSelectorCalendar.SelectedRanges)
                    DateRangeSelected?.Invoke(this, new DateRangeSelectedEventArgs(new DateRange(range.Start, range.End)));
            }
        }
        private void Click_ProfileButton(object sender, RoutedEventArgs e)
        {
            if (!e.Handled)
            {
                ProfileSelectedAction?.Invoke(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void Click_CategoryButton(object sender, RoutedEventArgs e)
        {
            if (!e.Handled)
            {
                if (sender == AnalysisButton)
                    CategorySelectedAction?.Invoke(this, new CategorySelectedEventArgs(CategorySelectedEventArgs.CategoryType.Analysis));
                else if (sender == PlanButton)
                    CategorySelectedAction?.Invoke(this, new CategorySelectedEventArgs(CategorySelectedEventArgs.CategoryType.Plan));
                else if (sender == NewEntryButton)
                    CategorySelectedAction?.Invoke(this, new CategorySelectedEventArgs(CategorySelectedEventArgs.CategoryType.NewEntry));

                e.Handled = true;
            }
        }

        private void Selected_BankAccount(object sender, MouseButtonEventArgs e)
        {
            if (BankAccountsDisplayer.SelectedIndex >= 0 && BankAccountsDisplayer.SelectedIndex < BankAccounts.Count)
                BankAccountSelected?.Invoke(this, new BankAccountSelectedEventArgs(BankAccounts[BankAccountsDisplayer.SelectedIndex], BankAccountsDisplayer.SelectedIndex));
        }
        #endregion
    }
}
