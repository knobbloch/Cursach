using System;

namespace ApplicationProjectViews
{
    public class BankAccountInfo
    {
        public BankAccountInfo()
        {
            AccountName = "";
            AccountBalance = 0;
            CurrencyIdentifier = "";
        }

        /// <summary>
        /// Contains the displayed name of the account
        /// </summary>
        public string AccountName
        {
            get => m_AccountName;
            init => m_AccountName = value ?? throw new ArgumentNullException(nameof(AccountName));
        }
        private string m_AccountName;
        /// <summary>
        /// Contains the account's balance
        /// </summary>
        public decimal AccountBalance { get; init; }

        public string CurrencyIdentifier
        {
            get => m_CurrencyIdentifier;
            init => m_CurrencyIdentifier = value ?? throw new ArgumentNullException(nameof(CurrencyIdentifier));
        }
        private string m_CurrencyIdentifier;
    }
}
