using System;
using System.Globalization;
using System.ComponentModel;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Controls;

using ApplicationProjectViews;
using ApplicationProjectViews.PlanPageView;

namespace ApplicationProject.UserControls.PlanPageView
{
    /// <summary>
    /// Interaction logic for PlanPageView.xaml
    /// </summary>
    public partial class PlanPageView : UserControl, IPlanPageView, INotifyPropertyChanged, ICultureDependentData
    {
        protected static readonly Point CalendarOffset = new(0, 0);
        protected const string ExpensesTabNameKey = "PAGE_PLAN_TAB_EXPENSES_NAME";
        protected const string IncomeTabNameKey = "PAGE_PLAN_TAB_INCOME_NAME";
        protected const string ExpensesTableNameHeaderKey = "PAGE_PLAN_TAB_EXPENSES_TABLE_HEADER_NAME";
        protected const string ExpensesTableRealValueHeaderKey = "PAGE_PLAN_TAB_EXPENSES_TABLE_HEADER_REALVALUE";
        protected const string ExpensesTablePlannedValueHeaderKey = "PAGE_PLAN_TAB_EXPENSES_TABLE_HEADER_PLANNEDVALUE";
        protected const string IncomeTableNameHeaderKey = "PAGE_PLAN_TAB_INCOME_TABLE_HEADER_NAME";
        protected const string IncomeTableRealValueHeaderKey = "PAGE_PLAN_TAB_INCOME_TABLE_HEADER_REALVALUE";
        protected const string IncomeTablePlannedValueHeaderKey = "PAGE_PLAN_TAB_INCOME_TABLE_HEADER_PLANNEDVALUE";
        protected const string AddExpenseCategoryTextKey = "PAGE_PLAN_TAB_EXPENSES_BUTTON_ADDCATEGORY";
        protected const string AddIncomeCategoryTextKey = "PAGE_PLAN_TAB_INCOME_BUTTON_ADDCATEGORY";

        public PlanPageView()
        {
            IncomeItems = new ObservableCollection<PlanPageIncomeEntry>();
            ExpensesItems = new ObservableCollection<PlanPageExpenseEntry>();

            InitializeComponent();

            CurrentCulture = null;
        }

        private CultureInfo m_CurrentCulture;
        protected CultureInfo CurrentCulture
        {
            get => m_CurrentCulture;
            set
            {
                m_CurrentCulture = value ?? System.Threading.Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture ?? CultureInfo.CurrentCulture ?? CultureInfo.InvariantCulture;
                RefreshLocalization();
            }
        }

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

            return ExpensesTableNameHeader.Length > 0 &&
                   ExpensesTableRealValueHeader.Length > 0 &&
                   ExpensesTablePlannedValueHeader.Length > 0 &&
                   IncomeTableNameHeader.Length > 0 &&
                   IncomeTableRealValueHeader.Length > 0 &&
                   IncomeTablePlannedValueHeader.Length > 0 &&
                   ExpensesTabName.Length > 0 &&
                   IncomeTabName.Length > 0 &&
                   AddExpenseCategoryText.Length > 0;
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

        #region IPlanPageView
        public string ExpensesTabName => GetLocalizedString(ExpensesTabNameKey);
        public string IncomeTabName => GetLocalizedString(IncomeTabNameKey);
        public string ExpensesTableNameHeader => GetLocalizedString(ExpensesTableNameHeaderKey);
        public string ExpensesTablePlannedValueHeader => GetLocalizedString(ExpensesTablePlannedValueHeaderKey);
        public string ExpensesTableRealValueHeader => GetLocalizedString(ExpensesTableRealValueHeaderKey);
        public string IncomeTableNameHeader => GetLocalizedString(IncomeTableNameHeaderKey);
        public string IncomeTablePlannedValueHeader => GetLocalizedString(IncomeTablePlannedValueHeaderKey);
        public string IncomeTableRealValueHeader => GetLocalizedString(IncomeTableRealValueHeaderKey);
        public string AddExpenseCategoryText => GetLocalizedString(AddExpenseCategoryTextKey);
        public string AddIncomeCategoryText => GetLocalizedString(AddIncomeCategoryTextKey);


        public event EventHandler AddExpenseCategoryAction;
        public event PlanPageModeSelectedEventHandler ModeChanged;
        public event PlanPageIncomeEntrySelectedEventHandler IncomeEntrySelected;
        public event PlanPageExpenseEntrySelectedEventHandler ExpenseEntrySelected;
        public event EventHandler AddIncomeCategoryAction;
        public IPlanPageView.PlanPageMode CurrentMode
        {
            get => m_CurrentMode;
            set
            {
                TabsControl.SelectedIndex = value switch
                {
                    IPlanPageView.PlanPageMode.Expenses => 0,
                    IPlanPageView.PlanPageMode.Income => 1,
                    _ => throw new ArgumentOutOfRangeException(nameof(CurrentMode))
                };

                m_CurrentMode = value;
            }
        }
        private IPlanPageView.PlanPageMode m_CurrentMode;
        public ICollection<PlanPageIncomeEntry> IncomeItems { get; }
        public ICollection<PlanPageExpenseEntry> ExpensesItems { get; }
        #endregion

        #region Methods
        public void RefreshLocalization()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTabName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTabName)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTableNameHeader)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTableRealValueHeader)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTablePlannedValueHeader)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTableNameHeader)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTableRealValueHeader)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTablePlannedValueHeader)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddExpenseCategoryText)));
        }

        private string GetLocalizedString(string key)
        {
            return ApplicationProject.Resources.Locale.ResourceManager.GetString(key, CurrentCulture);
        }
        #endregion

        #region Handled events
        private void AddExpenseCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseCategoryAction?.Invoke(this, EventArgs.Empty);
        }

        private void TabsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModeChanged?.Invoke(this, new PlanPageModeSelectedEventArgs(TabsControl.SelectedIndex switch
            {
                0 => IPlanPageView.PlanPageMode.Expenses,
                1 => IPlanPageView.PlanPageMode.Income,
                _ => throw new InvalidOperationException("Invalid tab was selected")
            }));
        }

        private void ExpensesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExpenseEntrySelected?.Invoke(this, new PlanPageExpenseEntrySelectedEventArgs((PlanPageExpenseEntry)ExpensesList.SelectedItem));
        }

        private void IncomeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IncomeEntrySelected?.Invoke(this, new PlanPageIncomeEntrySelectedEventArgs((PlanPageIncomeEntry)IncomeList.SelectedItem));
        }

        private void AddIncomeCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddIncomeCategoryAction?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
