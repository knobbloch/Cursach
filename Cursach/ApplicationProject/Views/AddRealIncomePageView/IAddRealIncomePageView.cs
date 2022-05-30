using System;
using System.Collections.Generic;

namespace ApplicationProject.Views.AddIncomePageView
{
    public interface IAddRealIncomePageView : IBaseView
    {
        /// <summary>
        /// Is called when the "add" action should be executed
        /// </summary>
        event EventHandler AddAction;

        /// <summary>
        /// Is called when the user wants to stop adding a new category
        /// </summary>
        event EventHandler ExitAction;

        /// <summary>
        /// The name of the income currently entered
        /// </summary>
        string IncomeName { get; }

        /// <summary>
        /// ValueInputError for IncomeName
        /// </summary>
        ValueInputError IncomeNameError { get; }

        /// <summary>
        /// The amount of money
        /// </summary>
        decimal CurrencyAmount { get; set; }

        /// <summary>
        /// TheValueInputError for CurrencyAmount
        /// </summary>
        ValueInputError CurrencyAmountError { get; }

        /// <summary>
        /// The currently selected bank account
        /// </summary>
        BankAccountInfo SelectedBankAccount { get; }

        /// <summary>
        /// The bank accounts to display
        /// </summary>
        ICollection<BankAccountInfo> BankAccounts { get; }
    }
}
