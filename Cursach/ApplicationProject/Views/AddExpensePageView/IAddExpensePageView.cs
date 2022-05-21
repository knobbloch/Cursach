using System;
using System.Collections.Generic;

using ApplicationProject.Views.InterPageView;

namespace ApplicationProject.Views.AddExpensePageView
{
    public interface IAddExpensePageView : IInterPageView
    {
        /// <summary>
        /// Is called when the "add" action should be executed
        /// </summary>
        event EventHandler AddAction;

        /// <summary>
        /// The name of the expense currently entered
        /// </summary>
        string ExpenseName { get; set; }

        /// <summary>
        /// The ValueInputError for ExpenseName
        /// </summary>
        ValueInputError ExpenseNameError { get; }

        /// <summary>
        /// The currently selected category
        /// </summary>
        CategoryDescriptor SelectedExpenseCategory { get; set; }

        /// <summary>
        /// The currently selected bank account
        /// </summary>
        BankAccountInfo SelectedExpenseBankAccount { get; set; }

        /// <summary>
        /// The currently entered amount of money
        /// </summary>
        int MoneyAmount { get; set; }

        /// <summary>
        /// TheValueInputError for MoneyAmount
        /// </summary>
        ValueInputError MoneyAmountError { get; }

        /// <summary>
        /// The categories to display to select from
        /// </summary>
        ICollection<CategoryDescriptor> ExpenseCategories { get; }

        /// <summary>
        /// The bank accounts to display to select from
        /// </summary>
        ICollection<BankAccountInfo> ExpenseBankAccounts { get; }
    }
}
