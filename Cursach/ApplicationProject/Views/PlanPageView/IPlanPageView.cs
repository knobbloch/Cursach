using System;
using ApplicationProject.Views.DatedPageView;

namespace ApplicationProject.Views.AnalysisPageView
{
    public interface IPlanPageView : IDatedPageView
    {
        /// <summary>
        /// Is called when the "add expense category" button is clicked
        /// </summary>
        event EventHandler AddExpenseCategoryClicked;
        /// <summary>
        /// Is called when the "create expenses report" button is clicked
        /// </summary>
        event EventHandler CreateExpensesReportClicked;
        /// <summary>
        /// Is called when the "add income" button is clicked
        /// </summary>
        event EventHandler AddIncomeClicked;
        /// <summary>
        /// Is called when the "Create income report" button is clicked
        /// </summary>
        event EventHandler CreateIncomeReportClicked;
    }
}