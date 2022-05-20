using System;
using System.Collections.Generic;

namespace ApplicationProject.Views.AddIncomePageView
{
    public interface IAddIncomePageView : IBaseView
    {
        /// <summary>
        /// Is called when the "add" button is clicked
        /// </summary>
        event EventHandler AddButtonClicked;

        /// <summary>
        /// The name of the expense currently entered
        /// </summary>
        string IncomeName { get; }

        /// <summary>
        /// The currently selected bank account
        /// </summary>
        BankAccountInfo SelectedBankAccount { get; }

        /// <summary>
        /// The currently entered amount of money
        /// </summary>
        int MoneyAmount { get; }

        /// <summary>
        /// The bank accounts to display
        /// </summary>
        ICollection<BankAccountInfo> BankAccounts { get; }
    }
}
