using System;
using System.Collections.Generic;

using ApplicationProjectViews.DatedPageView;

namespace ApplicationProjectViews.AnalysisPageView
{
    public interface IAnalysisPageView : IBaseView
    {
        enum AnalysisPageMode
        {
            Expenses,
            Income
        }

        /// <summary>
        /// Is called when the "add expense" action should be executed
        /// </summary>
        event EventHandler AddExpenseAction;
        /// <summary>
        /// Is called when the "add expense category" action should be executed
        /// </summary>
        event EventHandler AddExpenseCategoryAction;
        /// <summary>
        /// Is called when the "add income" action should be executed
        /// </summary>
        event EventHandler AddIncomeAction;
        /// <summary>
        /// Is called when the "add income category" action should be executed
        /// </summary>
        event EventHandler AddIncomeCategoryAction;
        /// <summary>
        /// Is called when the active mode is changed
        /// </summary>
        event AnalysisPageModeSelectedEventHandler ModeChanged;
        /// <summary>
        /// Is called when an income entry is selected
        /// </summary>
        event AnalysisPageIncomeEntrySelectedEventHandler IncomeEntrySelected;
        /// <summary>
        /// Is called when an expense entry is selected
        /// </summary>
        event AnalysisPageExpenseEntrySelectedEventHandler ExpenseEntrySelected;

        /// <summary>
        /// Manages the currently selected mode of the page
        /// </summary>
        public AnalysisPageMode CurrentMode { get; set; }
        /// <summary>
        /// Stores items which are used to build the income chart
        /// </summary>
        public ICollection<AnalysisPageIncomeDayEntry> IncomeDays{ get; }
        /// <summary>
        /// Stores items which are used to build expenses chart
        /// </summary>
        public ICollection<AnalysisPageExpenseDayEntry> ExpensesDays { get; }
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