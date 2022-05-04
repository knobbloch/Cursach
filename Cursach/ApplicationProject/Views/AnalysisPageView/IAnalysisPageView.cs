using System;
using System.Collections.Generic;
using ApplicationProject.Views.DatedPageView;

namespace ApplicationProject.Views.AnalysisPageView
{
    public interface IAnalysisPageView : IDatedPageView
    {
        enum AnalysisPageTab
        {
            Expenses,
            Income
        }

        /// <summary>
        /// Is called when the "add expense" button is clicked
        /// </summary>
        event EventHandler AddExpenseClicked;
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

        /// <summary>
        /// Is called when the active tab is changed
        /// </summary>
        event EventHandler<AnalysisPageTabSelectedEventArgs> TabChanged;

        /// <summary>
        /// Manages the currently selected tab of the page
        /// </summary>
        public AnalysisPageTab ActiveTab { get; set; }
        /// <summary>
        /// Stores items which are used to build the income chart
        /// </summary>
        public ICollection<AnalysisPageChartIncomeEntry> IncomeChartItems { get; }
        /// <summary>
        /// Stores items which are used to build expenses chart
        /// </summary>
        public ICollection<AnalysisPageChartExpenseEntry> ExpensesChartItems { get; }
        /// <summary>
        /// Stores items which are used to build income table
        /// </summary>
        public ICollection<AnalysisPageIncomeEntry> IncomeItems { get; }
        /// <summary>
        /// Stores items which are used to build expenses table
        /// </summary>
        public ICollection<AnalysisPageExpenseEntry> ExpenesItems { get; }
    }
}