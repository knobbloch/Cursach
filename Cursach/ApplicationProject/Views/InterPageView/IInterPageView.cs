using System;
using System.Collections.Generic;

namespace ApplicationProject.Views.InterPageView
{
    public interface IInterPageView : IBaseView
    {
        /// <summary>
        /// Is called when one of the categories is selected
        /// </summary>
        event CategorySelectedEventHandler CategorySelectedAction;
        /// <summary>
        /// Is called when the "profile selected" action should be executed
        /// </summary>
        event EventHandler ProfileSelectedAction;
        /// <summary>
        /// Is called when a bank account is seleced
        /// </summary>
        event BankAccountSelectedSelectedEventHandler BankAccountSelected;

        /// <summary>
        /// Stores instance of bank accounts
        /// </summary>
        IList<BankAccountInfo> BankAccounts { get; }

        /// <summary>
        /// Updates the account button's key
        /// </summary>
        string AccountName { set; }
    }
}
