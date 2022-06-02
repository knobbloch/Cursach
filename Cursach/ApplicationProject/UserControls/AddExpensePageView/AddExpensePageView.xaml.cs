using System;
using System.Globalization;
using System.ComponentModel;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Controls;

using ApplicationProject.Views;
using ApplicationProject.Views.AddExpensePageView;

namespace ApplicationProject.UserControls.AddExpensePageView
{
    /// <summary>
    /// Interaction logic for AddExpensePageView.xaml
    /// </summary>
    public partial class AddExpensePageView : UserControl, IAddExpensePageView, INotifyPropertyChanged
    {
        protected const string ExpenseNameFieldTextKey = "PAGE_ADDEXPENSE_NAMEFIELD_NAME";
        protected const string CurrencyAmountFieldTextKey = "PAGE_ADDEXPENSE_CURRENCYAMOUNTFIELD_NAME";
        protected const string ExpenseCategoryFieldTextKey = "PAGE_ADDEXPENSE_EXPENSECATEGORYFIELD_NAME";
        protected const string BankAccountFieldTextKey = "PAGE_ADDEXPENSE_BANKACCOUNTFIELD_NAME";
        protected const string ButtonAddTextKey = "PAGE_ADDEXPENSE_BUTTONADD_NAME";
        protected const string ButtonExitTextKey = "PAGE_ADDEXPENSE_BUTTONEXIT_NAME";

        public AddExpensePageView()
        {
            m_ExpenseName = "";
            m_CurrencyAmount = 0;
            CurrentCulture = null;


            ExpenseCategories = new ObservableCollection<CategoryDescriptor>();
            ExpenseBankAccounts = new ObservableCollection<BankAccountInfo>();

            InitializeComponent();
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

        public string ExpenseNameFieldText => GetLocalizedString(ExpenseNameFieldTextKey);
        public string CurrencyAmountFieldText => GetLocalizedString(CurrencyAmountFieldTextKey);
        public string ExpenseCategoryFieldText => GetLocalizedString(ExpenseCategoryFieldTextKey);
        public string BankAccountFieldText => GetLocalizedString(BankAccountFieldTextKey);
        public string ButtonAddText => GetLocalizedString(ButtonAddTextKey);
        public string ButtonExitText => GetLocalizedString(ButtonExitTextKey);

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

        #region IAddExpensePageView
        public event EventHandler AddAction;
        public event EventHandler AddActionPost;
        public event EventHandler ExitAction;

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
                ExpenseNameErrorText.Visibility = value != null ? Visibility.Visible : Visibility.Hidden;
                ExpenseNameErrorText.Text = value?.ToString() ?? "";
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
                CurrencyAmountErrorText.Visibility = value != null ? Visibility.Visible : Visibility.Hidden;
                CurrencyAmountErrorText.Text = value?.ToString() ?? "";
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

        public ICollection<BankAccountInfo> ExpenseBankAccounts { get; }

        public BankAccountInfo SelectedExpenseBankAccount
        {
            get => m_SelectedExpenseBankAccount;
            set
            {
                m_SelectedExpenseBankAccount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedExpenseBankAccount)));
            }
        }
        private BankAccountInfo m_SelectedExpenseBankAccount;
        #endregion

        #region Methods
        public void RefreshLocalization()
        {
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

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAction?.Invoke(this, EventArgs.Empty);
            AddActionPost?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            ExitAction?.Invoke(this, EventArgs.Empty);
        }
    }
}
