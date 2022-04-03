using System.ComponentModel;

namespace ApplicationProject.Views.InterPageView
{
    public class BankAccountInfo : INotifyPropertyChanged
    {
        public BankAccountInfo(string accountName, string accountBalance)
        {
            m_AccountName = accountName;
            m_AccountBalance = accountBalance;
        }

        /// <summary>
        /// Contains the displayed name of the account
        /// </summary>
        public string AccountName
        {
            get => m_AccountName;
            set
            {
                m_AccountName = value ?? "";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountName)));
            }
        }
        protected string m_AccountName;
        /// <summary>
        /// Contains the account's balance
        /// </summary>
        public string AccountBalance
        {
            get => m_AccountBalance;
            set
            {
                m_AccountBalance = value ?? "";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountBalance)));
            }
        }
        protected string m_AccountBalance;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
