using System;

namespace ApplicationProjectViews.AddBankAccountPageView
{
    public interface IAddBankAccountPageView : IBaseView
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
        /// The name of the bank account currently entered
        /// </summary>
        string AccountName { get; set; }

        /// <summary>
        /// ValueInputError for AccountName
        /// </summary>
        ValueInputError AccountNameError { get; }

        /// <summary>
        /// The current amount of currency
        /// </summary>
        decimal CurrencyAmount { get; set; }

        /// <summary>
        /// ValueInputError for CurrencyAmount
        /// </summary>
        ValueInputError CurrencyAmountError { get; }
    }
}
