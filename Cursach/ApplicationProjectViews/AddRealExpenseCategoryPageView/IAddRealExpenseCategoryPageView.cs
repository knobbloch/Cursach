using System;

using ApplicationProjectViews.InterPageView;

namespace ApplicationProjectViews.AddRealExpenseCategoryPageView
{
    public interface IAddRealExpenseCategoryPageView : IInterPageView
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
        /// The name of the category currently entered
        /// </summary>
        string CategoryName { get; set; }

        /// <summary>
        /// ValueInputError for CategoryName
        /// </summary>
        ValueInputError CategoryNameError { get; }

        /// <summary>
        /// A path to an image for the category
        /// </summary>
        string CategoryImagePath { get; set; }

        /// <summary>
        /// ValueInputError for CategoryImagePath
        /// </summary>
        ValueInputError CategoryImagePathError { get; }
    }
}
