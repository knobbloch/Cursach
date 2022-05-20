using System;
using System.Collections.Generic;

namespace ApplicationProject.Views.AddExpenseCategoryPageView
{
    public interface IAddExpenseCategoryPageView : IBaseView
    {
        /// <summary>
        /// Is called when the "add" button is clicked
        /// </summary>
        event EventHandler AddButtonClicked;

        /// <summary>
        /// The name of the category currently entered
        /// </summary>
        string CategoryName { get; }
    }
}
