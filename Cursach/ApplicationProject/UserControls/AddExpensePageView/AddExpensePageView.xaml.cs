using System;
using System.Globalization;
using System.ComponentModel;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Controls;

using ApplicationProjectViews;
using ApplicationProjectViews.AddExpensePageView;

namespace ApplicationProject.UserControls.AddExpensePageView
{
    /// <summary>
    /// Interaction logic for AddExpensePageView.xaml
    /// </summary>
    public partial class AddExpensePageView : UserControl, IAddExpensePageView, INotifyPropertyChanged, ICultureDependentData, ISupportOverlay
    {
        protected static readonly Point CalendarOffset = new(0, 0);

        protected const string ExpenseNameFieldTextKey = "PAGE_ADDEXPENSE_NAMEFIELD_NAME";
        protected const string CurrencyAmountFieldTextKey = "PAGE_ADDEXPENSE_CURRENCYAMOUNTFIELD_NAME";
        protected const string ExpenseCategoryFieldTextKey = "PAGE_ADDEXPENSE_EXPENSECATEGORYFIELD_NAME";
        protected const string BankAccountFieldTextKey = "PAGE_ADDEXPENSE_BANKACCOUNTFIELD_NAME";
        protected const string ButtonAddTextKey = "PAGE_ADDEXPENSE_BUTTONADD_NAME";
        protected const string ButtonExitTextKey = "PAGE_ADDEXPENSE_BUTTONEXIT_NAME";
        protected const string DateFieldTextKey = "PAGE_ADDEXPENSE_DATEFIELD_NAME";

        public AddExpensePageView()
        {
            m_ExpenseName = "";
            m_CurrencyAmount = 0;
            CurrentCulture = null;

            DateSelectorRoot = new Viewbox
            {
                Child = new RangeSelectorCalendar()
            };

            DateSelectorCalendar = (RangeSelectorCalendar)DateSelectorRoot.Child;
            DateSelectorCalendar.SelectionTarget = RangeSelectorCalendar.RangeSelectorCalendarMode.Day;
            DateSelectorCalendar.SelectionMode = RangeSelectorCalendar.RangeSelectorSelectionMode.Single;
            DateSelectorCalendar.SelectionChanged += DateSelectorCalendar_SelectionChanged;
            ExpenseCategories = new ObservableCollection<CategoryDescriptor>();
            BankAccounts = new ObservableCollection<BankAccountInfo>();

            InitializeComponent();
        }

        protected Viewbox DateSelectorRoot { get; }
        protected RangeSelectorCalendar DateSelectorCalendar { get; }

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

        public string ExpenseNameFieldText => GetLocalizedString(ExpenseNameFieldTextKey);
        public string CurrencyAmountFieldText => GetLocalizedString(CurrencyAmountFieldTextKey);
        public string ExpenseCategoryFieldText => GetLocalizedString(ExpenseCategoryFieldTextKey);
        public string BankAccountFieldText => GetLocalizedString(BankAccountFieldTextKey);
        public string ButtonAddText => GetLocalizedString(ButtonAddTextKey);
        public string ButtonExitText => GetLocalizedString(ButtonExitTextKey);
        public string DateFieldText => GetLocalizedString(DateFieldTextKey);

        #region IBaseView
        public bool Show()
        {
            ShowPreview?.Invoke(this, EventArgs.Empty);

            return true;
        }


        public void OnCultureChanged(CultureInfo newCulture)
        {
            CurrentCulture = newCulture;
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

        #region ISupportOverlay
        private Overlay m_Overlay;
        public Overlay Overlay
        {
            get => m_Overlay;
            set
            {
                m_Overlay = value;

                if (m_Overlay != null)
                    m_Overlay.BackgroundClick += Overlay_Click;
            }
        }

        public void ClearOverlay()
        {
            if (Overlay.Visible)
                Overlay.RemoveElement(DateSelectorRoot);

            Overlay.BackgroundClick -= Overlay_Click;
        }
        #endregion

        #region IAddExpensePageView
        public event EventHandler AddAction;
        public event EventHandler AddActionPost;
        public event EventHandler ExitAction;
        public event EventHandler SelectedDateChanged;

        public string ExpenseName
        {
            get => m_ExpenseName;
            set
            {
                m_ExpenseName = value ?? throw new ArgumentNullException(nameof(ExpenseName));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpenseName)));
            }
        }
        private string m_ExpenseName;

        public ValueInputError ExpenseNameError
        {
            get => m_ExpenseNameError;
            set
            {
                m_ExpenseNameError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpenseNameError)));
            }
        }
        private ValueInputError m_ExpenseNameError;

        public decimal CurrencyAmount
        {
            get => m_CurrencyAmount;
            set
            {
                m_CurrencyAmount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyAmount)));
            }
        }
        private decimal m_CurrencyAmount;

        public ValueInputError CurrencyAmountError
        {
            set
            {
                m_CurrencyAmountError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyAmountError)));
            }
            get => m_CurrencyAmountError;
        }
        private ValueInputError m_CurrencyAmountError;

        public ICollection<CategoryDescriptor> ExpenseCategories { get; }

        public CategoryDescriptor SelectedExpenseCategory
        {
            get => m_SelectedExpenseCategory;
            set
            {
                m_SelectedExpenseCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedExpenseCategory)));
            }
        }
        private CategoryDescriptor m_SelectedExpenseCategory;

        public ICollection<BankAccountInfo> BankAccounts { get; }

        public BankAccountInfo SelectedBankAccount
        {
            get => m_SelectedBankAccount;
            set
            {
                m_SelectedBankAccount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBankAccount)));
            }
        }
        private BankAccountInfo m_SelectedBankAccount;

        public DateTime SelectedDate
        {
            get => m_SelectedDate;
            set
            {
                m_SelectedDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDate)));
            }
        }
        private DateTime m_SelectedDate;
        #endregion

        #region Methods
        public void RefreshLocalization()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpenseNameFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyAmountFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpenseCategoryFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BankAccountFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonAddText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonExitText)));
        }

        private string GetLocalizedString(string key)
        {
            return ApplicationProject.Resources.Locale.ResourceManager.GetString(key, CurrentCulture);
        }
        #endregion

        #region Handled Events
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAction?.Invoke(this, EventArgs.Empty);
            AddActionPost?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            ExitAction?.Invoke(this, EventArgs.Empty);
        }

        private void DateSelector_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == DateSelector)
            {
                Overlay.AddElement(DateSelectorRoot);
                DateSelectorRoot.Height = DateSelectorRoot.Width = Math.Min(DateSelector.ActualWidth, 300);
                Overlay.MoveElement(DateSelectorRoot, DateSelector, new Point(CalendarOffset.X, CalendarOffset.Y + DateSelector.ActualHeight));
                Overlay.Visible = DateSelector.IsChecked ?? false;
                DateSelectorRoot.Visibility = (DateSelector.IsChecked ?? false) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void Overlay_Click(object sender, EventArgs e)
        {
            Overlay.Visible = false;
            DateSelector.IsChecked = false;
            m_Overlay.RemoveElement(DateSelectorRoot);
        }

        private void CurrentPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Overlay.Visible)
            {
                DateSelectorRoot.Height = DateSelectorRoot.Width = Math.Min(DateSelector.ActualWidth, 300);
                Overlay.MoveElement(DateSelectorRoot, DateSelector, new Point(CalendarOffset.X, CalendarOffset.Y + DateSelector.ActualHeight));
            }
        }

        private void DateSelectorCalendar_SelectionChanged(object sender, EventArgs e)
        {
            if (sender == DateSelectorCalendar)
            {
                IEnumerator<RangeSelectorCalendar.DateRange> enumerator = DateSelectorCalendar.SelectedRanges.GetEnumerator();
                if (enumerator.MoveNext())
                    SelectedDate = enumerator.Current.Start;

                Overlay_Click(this, e);

                SelectedDateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
