using System;
using System.Globalization;
using System.ComponentModel;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Controls;

using ApplicationProjectViews;
using ApplicationProjectViews.AddIncomePageView;

namespace ApplicationProject.UserControls.AddIncomePageView
{
    /// <summary>
    /// Interaction logic for AddExpensePageView.xaml
    /// </summary>
    public partial class AddIncomePageView : UserControl, IAddIncomePageView, INotifyPropertyChanged, ICultureDependentData, ISupportOverlay
    {
        protected static readonly Point CalendarOffset = new(0, 0);

        protected const string IncomeNameFieldTextKey = "PAGE_ADDINCOME_NAMEFIELD_NAME";
        protected const string CurrencyAmountFieldTextKey = "PAGE_ADDINCOME_CURRENCYAMOUNTFIELD_NAME";
        protected const string IncomeCategoryFieldTextKey = "PAGE_ADDINCOME_INCOMECATEGORYFIELD_NAME";
        protected const string BankAccountFieldTextKey = "PAGE_ADDINCOME_BANKACCOUNTFIELD_NAME";
        protected const string ButtonAddTextKey = "PAGE_ADDINCOME_BUTTONADD_NAME";
        protected const string ButtonExitTextKey = "PAGE_ADDINCOME_BUTTONEXIT_NAME";
        protected const string DateFieldTextKey = "PAGE_ADDINCOME_DATEFIELD_NAME";

        public AddIncomePageView()
        {
            m_IncomeName = "";
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
            IncomeCategories = new ObservableCollection<CategoryDescriptor>();
            BankAccounts = new ObservableCollection<BankAccountInfo>();

            InitializeComponent();
        }

        protected Viewbox DateSelectorRoot { get; }
        protected RangeSelectorCalendar DateSelectorCalendar { get; }

        public bool IsValid
        {
            get
            {
                return IncomeNameError == null &&
                       CurrencyAmountError == null &&
                       !Validation.GetHasError(CurrencyAmountBox) &&
                       !Validation.GetHasError(IncomeNameBox);
            }
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

        public string IncomeNameFieldText => GetLocalizedString(IncomeNameFieldTextKey);
        public string CurrencyAmountFieldText => GetLocalizedString(CurrencyAmountFieldTextKey);
        public string IncomeCategoryFieldText => GetLocalizedString(IncomeCategoryFieldTextKey);
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

        public string IncomeName
        {
            get => m_IncomeName;
            set
            {
                m_IncomeName = value ?? throw new ArgumentNullException(nameof(IncomeName));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeName)));
            }
        }
        private string m_IncomeName;

        public ValueInputError IncomeNameError
        {
            get => m_IncomeNameError;
            set
            {
                m_IncomeNameError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeNameError)));
            }
        }
        private ValueInputError m_IncomeNameError;

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

        public ICollection<CategoryDescriptor> IncomeCategories { get; }

        public CategoryDescriptor SelectedIncomeCategory
        {
            get => m_SelectedIncomeCategory;
            set
            {
                m_SelectedIncomeCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIncomeCategory)));
            }
        }
        private CategoryDescriptor m_SelectedIncomeCategory;

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeNameFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyAmountFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncomeCategoryFieldText)));
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
