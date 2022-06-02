using System;
using System.ComponentModel;

namespace ApplicationProjectViews
{
    public class BankAccountInfo : INotifyPropertyChanged
    {
        public BankAccountInfo(string accountName, decimal accountBalance, string currencyIdentifier)
        {
            m_AccountName = accountName;
            m_AccountBalance = accountBalance;
            CurrencyIdentifier = currencyIdentifier;
        }

        /// <summary>
        /// Contains the displayed name of the account
        /// </summary>
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
        /// <summary>
        /// Contains the account's balance
        /// </summary>
        public decimal AccountBalance
        {
            get => m_AccountBalance;
            set
            {
                m_AccountBalance = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountBalance)));
            }
        }
        private decimal m_AccountBalance;

        public string CurrencyIdentifier
        {
            get => m_CurrencyIdentifier;
            set
            {
                m_CurrencyIdentifier = value ?? throw new ArgumentNullException(nameof(CurrencyIdentifier));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyIdentifier)));
            }
        }
        private string m_CurrencyIdentifier;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
