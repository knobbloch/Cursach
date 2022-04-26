using System;
using System.Collections.Generic;

namespace ApplicationProject.Views.AddExpenseView
{
    public interface IAddExpensePageView : IBaseView
    {
        /// <summary>
        /// Is called when the "add" button is clicked
        /// </summary>
        event EventHandler AddButtonClicked;

        /// <summary>
        /// The name of the expense currently entered
        /// </summary>
        string ExpenseName { get; }

        /// <summary>
        /// The currently selected category
        /// </summary>
        CategoryDescriptor SelectedCategory { get; }

        /// <summary>
        /// The currently selected bank account
        /// </summary>
        BankAccountInfo SelectedBankAccount { get; }

        /// <summary>
        /// The currently entered amount of money
        /// </summary>
        int MoneyAmount { get; }

        /// <summary>
        /// The categories to display
        /// </summary>
        ICollection<CategoryDescriptor> Categories { get; }

        /// <summary>
        /// The bank accounts to display
        /// </summary>
        ICollection<BankAccountInfo> BankAccounts { get; }
    }
}
