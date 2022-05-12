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

using System.Globalization;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ApplicationProject.Views.PlanPageView;

namespace ApplicationProject.UserControls.PlanPageView
{
    /// <summary>
    /// Interaction logic for PlanPageView.xaml
    /// </summary>
    public partial class PlanPageView : UserControl, IPlanPageView, INotifyPropertyChanged
    {
        public PlanPageView()
        {
            m_ExpensesTabNameKey = "";
            m_IncomeTabNameKey = "";
            m_ExpensesTableNameHeaderKey = "";
            m_ExpensesTableRealValueHeaderKey = "";
            m_ExpensesTablePlannedValueHeaderKey = "";
            m_IncomeTableNameHeaderKey = "";
            m_IncomeTableRealValueHeaderKey = "";
            m_IncomeTablePlannedValueHeaderKey = "";
            m_AddExpenseCategoryTextKey = "";
            m_CreateExpensesReportTextKey = "";
            m_CreateIncomeReportTextKey = "";

            InitializeComponent();

            IncomeBarChart.BarsSource = IncomeChartItems = new ObservableCollection<PlanPageIncomeChartEntry>();
            ExpensesBarChart.BarsSource = ExpensesChartItems = new ObservableCollection<PlanPageExpenseChartEntry>();
            IncomeList.ItemsSource = IncomeItems = new ObservableCollection<PlanPageIncomeEntry>();
            ExpensesList.ItemsSource = ExpenesItems = new ObservableCollection<PlanPageExpenseEntry>();
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
                                     ExpensesTableRealValueHeader.Length > 0 &&
                                     ExpensesTablePlannedValueHeader.Length > 0 &&
                                     IncomeTableNameHeader.Length > 0 &&
                                     IncomeTableRealValueHeader.Length > 0 &&
                                     IncomeTablePlannedValueHeader.Length > 0 &&
                                     ExpensesTabName.Length > 0 &&
                                     IncomeTabName.Length > 0 &&
                                     AddExpenseCategoryText.Length > 0 &&
                                     CreateExpensesReportText.Length > 0 &&
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

        #region IPlanPageView
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

        public string ExpensesTablePlannedValueHeaderKey
        {
            get => m_ExpensesTablePlannedValueHeaderKey;
            set
            {
                m_ExpensesTablePlannedValueHeaderKey = value ?? throw new ArgumentNullException(nameof(ExpensesTablePlannedValueHeaderKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTablePlannedValueHeader)));
            }
        }
        private string m_ExpensesTablePlannedValueHeaderKey;
        public string ExpensesTablePlannedValueHeader => ExpensesTablePlannedValueHeaderKey;

        public string ExpensesTableRealValueHeaderKey
        {
            get => m_ExpensesTableRealValueHeaderKey;
            set
            {
                m_ExpensesTableRealValueHeaderKey = value ?? throw new ArgumentNullException(nameof(ExpensesTableRealValueHeaderKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpensesTableRealValueHeader)));
            }
        }
        private string m_ExpensesTableRealValueHeaderKey;
        public string ExpensesTableRealValueHeader => ExpensesTableRealValueHeaderKey;

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

        public string IncomeTablePlannedValueHeaderKey
        {
            get => m_IncomeTablePlannedValueHeaderKey;
            set
            {
                m_IncomeTablePlannedValueHeaderKey = value ?? throw new ArgumentNullException(nameof(IncomeTablePlannedValueHeaderKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTablePlannedValueHeader)));
            }
        }
        private string m_IncomeTablePlannedValueHeaderKey;
        public string IncomeTablePlannedValueHeader => IncomeTablePlannedValueHeaderKey;

        public string IncomeTableRealValueHeaderKey
        {
            get => m_IncomeTableRealValueHeaderKey;
            set
            {
                m_IncomeTableRealValueHeaderKey = value ?? throw new ArgumentNullException(nameof(IncomeTableRealValueHeaderKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeTableRealValueHeader)));
            }
        }
        private string m_IncomeTableRealValueHeaderKey;
        public string IncomeTableRealValueHeader => IncomeTableRealValueHeaderKey;

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

        public event EventHandler AddExpenseCategoryClicked;
        public event EventHandler CreateExpensesReportClicked;
        public event EventHandler CreateIncomeReportClicked;
        public event PlanPageTabSelectedEventHandler TabChanged;
        public event PlanPageIncomeEntrySelectedEventHandler IncomeEntrySelected;
        public event PlanPageExpenseEntrySelectedEventHandler ExpenseEntrySelected;

        public IPlanPageView.PlanPageTab ActiveTab
        {
            get => m_ActiveTab;
            set
            {
                TabsControl.SelectedIndex = value switch
                {
                    IPlanPageView.PlanPageTab.Expenses => 0,
                    IPlanPageView.PlanPageTab.Income => 1,
                    _ => throw new ArgumentOutOfRangeException(nameof(ActiveTab))
                };

                m_ActiveTab = value;
            }
        }
        private IPlanPageView.PlanPageTab m_ActiveTab;
        public ICollection<PlanPageIncomeChartEntry> IncomeChartItems { get; }
        public ICollection<PlanPageExpenseChartEntry> ExpensesChartItems { get; }
        public ICollection<PlanPageIncomeEntry> IncomeItems { get; }
        public ICollection<PlanPageExpenseEntry> ExpenesItems { get; }
        #endregion

        #region Handled events
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
            TabChanged?.Invoke(this, new PlanPageTabSelectedEventArgs(TabsControl.SelectedIndex switch
            {
                0 => IPlanPageView.PlanPageTab.Expenses,
                1 => IPlanPageView.PlanPageTab.Income,
                _ => throw new InvalidOperationException("Invalid tab was selected")
            }));
        }

        private void CreateIncomeReportButton_Click(object sender, RoutedEventArgs e)
        {
            CreateIncomeReportClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ExpensesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExpenseEntrySelected?.Invoke(this, new PlanPageExpenseEntrySelectedEventArgs((PlanPageExpenseEntry)ExpensesList.SelectedItem));
        }

        private void IncomeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IncomeEntrySelected?.Invoke(this, new PlanPageIncomeEntrySelectedEventArgs((PlanPageIncomeEntry)IncomeList.SelectedItem));
        }
        #endregion
    }
}
