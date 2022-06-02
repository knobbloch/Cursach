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
        public AddExpensePageView()
        {
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
            set
            {
            
            }
        }

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

            }
        }

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
            
        }
        #endregion
    }
}
