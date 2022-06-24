using System;
using System.Globalization;
using System.ComponentModel;
using System.IO;

using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

using ApplicationProjectViews;
using ApplicationProjectViews.AddBankAccountPageView;

namespace ApplicationProject.UserControls.AddBankAccountPageView
{
    /// <summary>
    /// Interaction logic for AddExpensePageView.xaml
    /// </summary>
    public partial class AddBankAccountPageView : UserControl, IAddBankAccountPageView, INotifyPropertyChanged, ICultureDependentData
    {
        protected const string AccountNameFieldTextKey = "PAGE_ADDBANKACCOUNT_NAMEFIELD_NAME";
        protected const string CurrencyAmountFieldTextKey = "PAGE_ADDBANKACCOUNT_CURRENCYAMOUNTFIELD_NAME";
        protected const string ButtonAddTextKey = "PAGE_ADDBANKACCOUNT_BUTTONADD_NAME";
        protected const string ButtonExitTextKey = "PAGE_ADDBANKACCOUNT_BUTTONEXIT_NAME";

        public AddBankAccountPageView()
        {
            m_AccountName = "";
            m_CurrencyAmount = "0";
            CurrentCulture = null;

            InitializeComponent();
        }

        public bool IsValid
        {
            get
            {
                return AccountNameError == null &&
                       CurrencyAmountError == null &&
                       !Validation.GetHasError(AccountNameBox) &&
                       !Validation.GetHasError(CurrencyAmountBox);
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

        public string AccountNameFieldText => GetLocalizedString(AccountNameFieldTextKey);
        public string CurrencyAmountFieldText => GetLocalizedString(CurrencyAmountFieldTextKey);
        public string ButtonAddText => GetLocalizedString(ButtonAddTextKey);
        public string ButtonExitText => GetLocalizedString(ButtonExitTextKey);

        #region IBaseView
        public bool Show()
        {
            ShowPreview?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public void OnShown() { }

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

        #region IAddExpenseCategoryPageView
        public event EventHandler AddAction;
        public event EventHandler AddActionPost;
        public event EventHandler ExitAction;

        public string AccountName
        {
            get => m_AccountName;
            set
            {
                m_AccountName = value ?? throw new ArgumentNullException(nameof(AccountName));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountName)));
            }
        }
        private string m_AccountName;

        public ValueInputError AccountNameError
        {
            get => m_AccountNameError;
            set
            {
                m_AccountNameError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountNameError)));
            }
        }
        private ValueInputError m_AccountNameError;

        public string CurrencyAmount
        {
            get => m_CurrencyAmount;
            set
            {
                m_CurrencyAmount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyAmount)));
            }
        }
        private string m_CurrencyAmount;

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
        #endregion

        #region Methods
        public void RefreshLocalization()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountNameFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyAmountFieldText)));
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
