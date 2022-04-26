using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ApplicationProject.Views.InterPageView
{
    public interface IInterPageView : IBaseView
    {
        /// <summary>
        /// Is called when one of the category buttons is clicked on
        /// </summary>
        event EventHandler<CategorySelectedEventArgs> CategorySelected;
        /// <summary>
        /// Is called when the profile button is clicked on
        /// </summary>
        event EventHandler ProfileSelected;
        /// <summary>
        /// Is called when a bank account is clicked on
        /// </summary>
        event EventHandler<BankAccountSelectedEventArgs> BankAccountSelected;

        /// <summary>
        /// Stores instance of bank accounts
        /// </summary>
        IList<BankAccountInfo> BankAccounts { get; }

        /// <summary>
        /// Used to present active page
        /// </summary>
        IViewPresenter PageViewPresenter { get; }

        /// <summary>
        /// Updates the analysis button's undertext
        /// </summary>
        string AnalysisButtonName { set; }

        /// <summary>
        /// Updates the plan button's undertext
        /// </summary>
        string PlanButtonName { set; }

        /// <summary>
        /// Updates the account button's undertext
        /// </summary>
        string AccountName { set; }
    }
}
