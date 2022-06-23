using System;
using System.Collections.Generic;

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
        /// Is called when selectedDate is changed
        /// </summary>
        event EventHandler SelectedDateChanged;

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
        string CurrencyAmount { get; set; }

        /// <summary>
        /// TheValueInputError for CurrencyAmount
        /// </summary>
        ValueInputError CurrencyAmountError { set; }

        /// <summary>
        /// The currently selected date for the expense
        /// </summary>
        DateTime SelectedDate { get; set; }

        /// <summary>
        /// The categories to display to select from
        /// </summary>
        ICollection<CategoryDescriptor> ExpenseCategories { get; }

        /// <summary>
        /// The currently selected category
        /// </summary>
        CategoryDescriptor SelectedExpenseCategory { get; set; }

        ValueInputError SelectedExpenseCategoryError { set; }

        /// <summary>
        /// The bank accounts to display to select from
        /// </summary>
        ICollection<BankAccountInfo> BankAccounts { get; }

        /// <summary>
        /// The currently selected bank account
        /// </summary>
        BankAccountInfo SelectedBankAccount { get; set; }

        ValueInputError SelectedBankAccountError { set; }
    }
}
