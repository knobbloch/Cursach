using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using System.Globalization;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ApplicationProject.Views.AnalysisPageView;

namespace ApplicationProject.UserControls.AnalysisPageView
{
    /// <summary>
    /// Interaction logic for AnalysisPageView.xaml
    /// </summary>
    public partial class AnalysisPageView : UserControl, IAnalysisPageView, INotifyPropertyChanged
    {
        public AnalysisPageView()
        {
            m_ExpensesTabNameKey = "";
            m_IncomeTabNameKey = "";
            m_ExpensesTableNameHeaderKey = "";
            m_ExpensesTableValueHeaderKey = "";
            m_IncomeTableNameHeaderKey = "";
            m_IncomeTableValueHeaderKey = "";
            m_AddExpenseTextKey = "";
            m_AddExpenseCategoryTextKey = "";
            m_CreateExpensesReportTextKey = "";
            m_AddIncomeTextKey = "";
            m_CreateIncomeReportTextKey = "";

            InitializeComponent();

            IncomeBarChart.BarsSource = IncomeChartItems = new ObservableCollection<AnalysisPageIncomeChartEntry>();
            ExpensesBarChart.BarsSource = ExpensesChartItems = new ObservableCollection<AnalysisPageExpenseChartEntry>();
            IncomeList.ItemsSource = IncomeItems = new ObservableCollection<AnalysisPageIncomeEntry>();
            ExpensesList.ItemsSource = ExpenesItems = new ObservableCollection<AnalysisPageExpenseEntry>();
        }

        protected CultureInfo CurrentCulture { get; set; }

        #region IBaseView
        public void Show()
        {
            Shown?.Invoke(this, EventArgs.Empty);
        }
        public void Hide()
        {
            Hidden?.Invoke(this, EventArgs.Empty);
        }
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
                                     CreateIncomeReportText.Length > 0;

        public void OnCultureChanged(CultureInfo culture)
        {
            CurrentCulture = culture;
        }

        public event EventHandler Shown;
        public event EventHandler Hidden;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region IAnalysisPageView
        public string ExpensesTabNameKey
        {
            get => m_ExpensesTabNameKey;
            set
            {
                m_ExpensesTabNameKey = value ?? throw new ArgumentNullException(nameof(ExpensesTabNameKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTabName)));
            }
        }
        private string m_ExpensesTabNameKey;
        public string ExpensesTabName => ExpensesTabNameKey;

        public string IncomeTabNameKey
        {
            get => m_IncomeTabNameKey;
            set
            {
                m_IncomeTabNameKey = value ?? throw new ArgumentNullException(nameof(IncomeTabNameKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTabName)));
            }
        }
        private string m_IncomeTabNameKey;
        public string IncomeTabName => IncomeTabNameKey;

        public string ExpensesTableNameHeaderKey
        {
            get => m_ExpensesTableNameHeaderKey;
            set
            {
                m_ExpensesTableNameHeaderKey = value ?? throw new ArgumentNullException(nameof(ExpensesTableNameHeaderKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTableNameHeader)));
            }
        }
        private string m_ExpensesTableNameHeaderKey;
        public string ExpensesTableNameHeader => ExpensesTableNameHeaderKey;

        public string ExpensesTableValueHeaderKey
        {
            get => m_ExpensesTableValueHeaderKey;
            set
            {
                m_ExpensesTableValueHeaderKey = value ?? throw new ArgumentNullException(nameof(ExpensesTableValueHeaderKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTableValueHeader)));
            }
        }
        private string m_ExpensesTableValueHeaderKey;
        public string ExpensesTableValueHeader => ExpensesTableValueHeaderKey;

        public string IncomeTableNameHeaderKey
        {
            get => m_IncomeTableNameHeaderKey;
            set
            {
                m_IncomeTableNameHeaderKey = value ?? throw new ArgumentNullException(nameof(IncomeTableNameHeaderKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTableNameHeader)));
            }
        }
        private string m_IncomeTableNameHeaderKey;
        public string IncomeTableNameHeader => IncomeTableNameHeaderKey;

        public string IncomeTableValueHeaderKey
        {
            get => m_IncomeTableValueHeaderKey;
            set
            {
                m_IncomeTableValueHeaderKey = value ?? throw new ArgumentNullException(nameof(IncomeTableValueHeaderKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTableValueHeader)));
            }
        }
        private string m_IncomeTableValueHeaderKey;
        public string IncomeTableValueHeader => IncomeTableValueHeaderKey;

        public string AddExpenseTextKey
        {
            get => m_AddExpenseTextKey;
            set
            {
                m_AddExpenseTextKey = value ?? throw new ArgumentNullException(nameof(AddExpenseTextKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddExpenseText)));
            }
        }
        private string m_AddExpenseTextKey;
        public string AddExpenseText => AddExpenseTextKey;

        public string AddExpenseCategoryTextKey
        {
            get => m_AddExpenseCategoryTextKey;
            set
            {
                m_AddExpenseCategoryTextKey = value ?? throw new ArgumentNullException(nameof(AddExpenseCategoryTextKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddExpenseCategoryText)));
            }
        }
        private string m_AddExpenseCategoryTextKey;
        public string AddExpenseCategoryText => AddExpenseCategoryTextKey;

        public string CreateExpensesReportTextKey
        {
            get => m_CreateExpensesReportTextKey;
            set
            {
                m_CreateExpensesReportTextKey = value ?? throw new ArgumentNullException(nameof(CreateExpensesReportTextKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreateExpensesReportText)));
            }
        }
        private string m_CreateExpensesReportTextKey;
        public string CreateExpensesReportText => CreateExpensesReportTextKey;

        public string AddIncomeTextKey
        {
            get => m_AddIncomeTextKey;
            set
            {
                m_AddIncomeTextKey = value ?? throw new ArgumentNullException(nameof(AddIncomeTextKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddIncomeText)));
            }
        }
        private string m_AddIncomeTextKey;
        public string AddIncomeText => AddIncomeTextKey;

        public string CreateIncomeReportTextKey
        {
            get => m_CreateIncomeReportTextKey;
            set
            {
                m_CreateIncomeReportTextKey = value ?? throw new ArgumentNullException(nameof(CreateIncomeReportTextKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreateIncomeReportText)));
            }
        }
        private string m_CreateIncomeReportTextKey;
        public string CreateIncomeReportText => CreateIncomeReportTextKey;

        public event EventHandler AddExpenseClicked;
        public event EventHandler AddExpenseCategoryClicked;
        public event EventHandler CreateExpensesReportClicked;
        public event EventHandler AddIncomeClicked;
        public event EventHandler CreateIncomeReportClicked;
        public event AnalysisPageTabSelectedEventHandler TabChanged;
        public event AnalysisPageIncomeEntrySelectedEventHandler IncomeEntrySelected;
        public event AnalysisPageExpenseEntrySelectedEventHandler ExpenseEntrySelected;

        public IAnalysisPageView.AnalysisPageTab ActiveTab
        {
            get => m_ActiveTab;
            set
            {
                TabsControl.SelectedIndex = value switch
                {
                    IAnalysisPageView.AnalysisPageTab.Expenses => 0,
                    IAnalysisPageView.AnalysisPageTab.Income => 1,
                    _ => throw new ArgumentOutOfRangeException(nameof(ActiveTab))
                };

                m_ActiveTab = value;
            }
        }
        private IAnalysisPageView.AnalysisPageTab m_ActiveTab;
        public ICollection<AnalysisPageIncomeChartEntry> IncomeChartItems { get; }
        public ICollection<AnalysisPageExpenseChartEntry> ExpensesChartItems { get; }
        public ICollection<AnalysisPageIncomeEntry> IncomeItems { get; }
        public ICollection<AnalysisPageExpenseEntry> ExpenesItems { get; }
        #endregion

        #region Handled events
        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseClicked?.Invoke(this, EventArgs.Empty);
        }

        private void AddExpenseCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseCategoryClicked?.Invoke(this, EventArgs.Empty);
        }

        private void CreateExpensesReportButton_Click(object sender, RoutedEventArgs e)
        {
            CreateExpensesReportClicked?.Invoke(this, EventArgs.Empty);
        }

        private void TabsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabChanged?.Invoke(this, new AnalysisPageTabSelectedEventArgs(TabsControl.SelectedIndex switch
            {
                0 => IAnalysisPageView.AnalysisPageTab.Expenses,
                1 => IAnalysisPageView.AnalysisPageTab.Income,
                _ => throw new InvalidOperationException("Invalid tab was selected")
            }));
        }

        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            AddIncomeClicked?.Invoke(this, EventArgs.Empty);
        }

        private void CreateIncomeReportButton_Click(object sender, RoutedEventArgs e)
        {
            CreateIncomeReportClicked?.Invoke(this, EventArgs.Empty);
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
