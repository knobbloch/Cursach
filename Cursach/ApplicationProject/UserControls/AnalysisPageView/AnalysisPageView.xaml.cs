using System;
using System.Globalization;
using System.ComponentModel;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Controls;

using ApplicationProjectViews;
using ApplicationProjectViews.AnalysisPageView;

namespace ApplicationProject.UserControls.AnalysisPageView
{
    /// <summary>
    /// Interaction logic for AnalysisPageView.xaml
    /// </summary>
    public partial class AnalysisPageView : UserControl, IAnalysisPageView, INotifyPropertyChanged, ICultureDependentData
    {
        protected static readonly Point CalendarOffset = new(0, 0);
        protected const string ExpensesTabNameKey = "PAGE_ANALYSIS_TAB_EXPENSES_NAME";
        protected const string IncomeTabNameKey = "PAGE_ANALYSIS_TAB_INCOME_NAME";
        protected const string ExpensesTableNameHeaderKey = "PAGE_ANALYSIS_TAB_EXPENSES_TABLE_HEADER_NAME";
        protected const string ExpensesTableValueHeaderKey = "PAGE_ANALYSIS_TAB_EXPENSES_TABLE_HEADER_VALUE";
        protected const string IncomeTableNameHeaderKey = "PAGE_ANALYSIS_TAB_INCOME_TABLE_HEADER_NAME";
        protected const string IncomeTableValueHeaderKey = "PAGE_ANALYSIS_TAB_INCOME_TABLE_HEADER_VALUE";
        protected const string AddExpenseTextKey = "PAGE_ANALYSIS_TAB_EXPENSES_BUTTON_ADD";
        protected const string AddExpenseCategoryTextKey = "PAGE_ANALYSIS_TAB_EXPENSES_BUTTON_ADDCATEGORY";
        protected const string AddIncomeTextKey = "PAGE_ANALYSIS_TAB_INCOME_BUTTON_ADD";

        public AnalysisPageView()
        {
            InitializeComponent();

            CurrentCulture = null;

            IncomeBarChart.BarsSource = IncomeDays = new ObservableCollection<AnalysisPageIncomeDayEntry>();
            ExpensesBarChart.BarsSource = ExpensesDays = new ObservableCollection<AnalysisPageExpenseDayEntry>();
            IncomeList.ItemsSource = IncomeItems = new ObservableCollection<AnalysisPageIncomeEntry>();
            ExpensesList.ItemsSource = ExpenesItems = new ObservableCollection<AnalysisPageExpenseEntry>();
        }

        protected CultureInfo CurrentCulture
        {
            get => m_CurrentCulture;
            set
            {
                m_CurrentCulture = value ?? System.Threading.Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture ?? CultureInfo.CurrentCulture ?? CultureInfo.InvariantCulture;

                RefreshLocalization();
            }
        }
        private CultureInfo m_CurrentCulture;

        #region ICultureDependentData
        public void OnCultureChanged(CultureInfo newCulture)
        {
            CurrentCulture = newCulture;
        }
        #endregion

        #region IBaseView
        public bool Show()
        {
            ShowPreview?.Invoke(this, EventArgs.Empty);

            return  ExpensesTableNameHeader.Length > 0 &&
                    ExpensesTableValueHeader.Length > 0 &&
                    IncomeTableNameHeader.Length > 0 &&
                    IncomeTableValueHeader.Length > 0 &&
                    ExpensesTabName.Length > 0 &&
                    IncomeTabName.Length > 0 &&
                    AddExpenseText.Length > 0 &&
                    AddExpenseCategoryText.Length > 0 &&
                    AddIncomeText.Length > 0;
        }

        public void DispatchUpdate(ViewUpdate action)
        {
            Dispatcher.Invoke(() => action(this));
        }

        public event EventHandler ShowPreview;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region IAnalysisPageView
        public string ExpensesTabName => GetLocalizedString(ExpensesTabNameKey);
        public string IncomeTabName => GetLocalizedString(IncomeTabNameKey);
        public string ExpensesTableNameHeader => GetLocalizedString(ExpensesTableNameHeaderKey);
        public string ExpensesTableValueHeader => GetLocalizedString(ExpensesTableValueHeaderKey);
        public string IncomeTableNameHeader => GetLocalizedString(IncomeTableNameHeaderKey);
        public string IncomeTableValueHeader => GetLocalizedString(IncomeTableValueHeaderKey);
        public string AddExpenseText => GetLocalizedString(AddExpenseTextKey);
        public string AddExpenseCategoryText => GetLocalizedString(AddExpenseCategoryTextKey);
        public string AddIncomeText => GetLocalizedString(AddIncomeTextKey);

        public event EventHandler AddExpenseAction;
        public event EventHandler AddExpenseCategoryAction;
        public event EventHandler AddIncomeAction;
        public event AnalysisPageModeSelectedEventHandler ModeChanged;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTabName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTabName)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTableNameHeader)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTableValueHeader)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTableNameHeader)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTableValueHeader)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddExpenseText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddExpenseCategoryText)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddIncomeText)));
        }

        private string GetLocalizedString(string key)
        {
            return ApplicationProject.Resources.Locale.ResourceManager.GetString(key, CurrentCulture);
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

        private void ExpensesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExpenseEntrySelected?.Invoke(this, new AnalysisPageExpenseEntrySelectedEventArgs((AnalysisPageExpenseEntry)ExpensesList.SelectedItem));
        }

        private void IncomeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IncomeEntrySelected?.Invoke(this, new AnalysisPageIncomeEntrySelectedEventArgs((AnalysisPageIncomeEntry)IncomeList.SelectedItem));
        }
        #endregion
    }
}
