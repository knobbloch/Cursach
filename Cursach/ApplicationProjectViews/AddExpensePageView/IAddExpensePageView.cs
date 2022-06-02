using System;
using System.Collections.Generic;

using ApplicationProjectViews.InterPageView;

namespace ApplicationProjectViews.AddExpensePageView
{
    public interface IAddExpensePageView : IBaseView
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
        /// The name of the expense currently entered
        /// </summary>
        string ExpenseName { get; set; }

        /// <summary>
        /// The ValueInputError for ExpenseName
        /// </summary>
        ValueInputError ExpenseNameError { set; }

        /// <summary>
        /// The planned amount of money
        /// </summary>
        decimal CurrencyAmount { get; set; }

        /// <summary>
        /// TheValueInputError for CurrencyAmount
        /// </summary>
        ValueInputError CurrencyAmountError { set; }

        /// <summary>
        /// The categories to display to select from
        /// </summary>
        ICollection<CategoryDescriptor> ExpenseCategories { get; }

        /// <summary>
        /// The currently selected category
        /// </summary>
        CategoryDescriptor SelectedExpenseCategory { get; set; }

        /// <summary>
        /// The bank accounts to display to select from
        /// </summary>
        ICollection<BankAccountInfo> ExpenseBankAccounts { get; }

        /// <summary>
        /// The currently selected bank account
        /// </summary>
        BankAccountInfo SelectedExpenseBankAccount { get; set; }
    }
}
