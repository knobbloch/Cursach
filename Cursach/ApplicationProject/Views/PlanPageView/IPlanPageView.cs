using System;
using System.Collections.Generic;

using ApplicationProject.Views.DatedPageView;

namespace ApplicationProject.Views.PlanPageView
{
    public interface IPlanPageView : IDatedPageView
    {
        enum PlanPageMode
        {
            Expenses,
            Income
        }

        /// <summary>
        /// Is called when the "add expense category" action should be executed
        /// </summary>
        event EventHandler AddExpenseCategoryAction;
        /// <summary>
        /// Is called when the "create expenses report" action should be executed
        /// </summary>
        event EventHandler CreateExpensesReportAction;
        /// <summary>
        /// Is called when the "Create income report" action should be executed
        /// </summary>
        event EventHandler CreateIncomeReportAction;
        /// <summary>
        /// Is called when the active mode is changed
        /// </summary>
        event PlanPageTabSelectedEventHandler ModeChanged;
        /// <summary>
        /// Is called when an income entry is selected
        /// </summary>
        event PlanPageIncomeEntrySelectedEventHandler IncomeEntrySelected;
        /// <summary>
        /// Is called when an expense entry is selected
        /// </summary>
        event PlanPageExpenseEntrySelectedEventHandler ExpenseEntrySelected;


        /// <summary>
        /// Manages the currently selected mode of the page
        /// </summary>
        public PlanPageMode CurrentMode { get; set; }
        /// <summary>
        /// Stores items which are used to build income table
        /// </summary>
        public ICollection<PlanPageIncomeEntry> IncomeItems { get; }
        /// <summary>
        /// Stores items which are used to build expenses table
        /// </summary>
        public ICollection<PlanPageExpenseEntry> ExpenesItems { get; }
    }
}