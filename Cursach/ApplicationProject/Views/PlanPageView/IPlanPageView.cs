using System;
using System.Collections.Generic;

namespace ApplicationProject.Views.PlanPageView
{
    public interface IPlanPageView : IBaseView
    {
        enum PlanPageTab
        {
            Expenses,
            Income
        }

        /// <summary>
        /// Sets the key for the name of the expenses tab
        /// </summary>
        public string ExpensesTabNameKey { set; }
        /// <summary>
        /// Sets the key for the name of the income tab
        /// </summary>
        public string IncomeTabNameKey { set; }
        /// <summary>
        /// Sets the key for the name of the "Name" header in the expenes table
        /// </summary>
        public string ExpensesTableNameHeaderKey { set; }
        /// <summary>
        /// Sets the key for the name of the "planned value" header in the expenes table
        /// </summary>
        public string ExpensesTablePlannedValueHeaderKey { set; }
        /// <summary>
        /// Sets the key for the name of the "real value" header in the expenes table
        /// </summary>
        public string ExpensesTableRealValueHeaderKey { set; }
        /// <summary>
        /// Sets the key for the name of the "Name" header in the income table
        /// </summary>
        public string IncomeTableNameHeaderKey { set; }
        /// <summary>
        /// Sets the key for the name of the "planned value" header in the expenes table
        /// </summary>
        public string IncomeTablePlannedValueHeaderKey { set; }
        /// <summary>
        /// Sets the key for the name of the "real value" header in the expenes table
        /// </summary>
        public string IncomeTableRealValueHeaderKey { set; }
        /// <summary>
        /// Sets the key for the text of the "add expense category" tab
        /// </summary>
        public string AddExpenseCategoryTextKey { set; }
        /// <summary>
        /// Sets the key for the text of the "create expenses report" tab
        /// </summary>
        public string CreateExpensesReportTextKey { set; }
        /// <summary>
        /// Sets the key for the text of the "create income report" tab
        /// </summary>
        public string CreateIncomeReportTextKey { set; }

        /// <summary>
        /// Is called when the "add expense category" button is clicked
        /// </summary>
        event EventHandler AddExpenseCategoryClicked;
        /// <summary>
        /// Is called when the "create expenses report" button is clicked
        /// </summary>
        event EventHandler CreateExpensesReportClicked;
        /// <summary>
        /// Is called when the "Create income report" button is clicked
        /// </summary>
        event EventHandler CreateIncomeReportClicked;
        /// <summary>
        /// Is called when the active tab is changed
        /// </summary>
        event PlanPageTabSelectedEventHandler TabChanged;
        /// <summary>
        /// Is called when an income entry is selected (clicked on)
        /// </summary>
        event PlanPageIncomeEntrySelectedEventHandler IncomeEntrySelected;
        /// <summary>
        /// Is called when an expense entry is selected (clicked on)
        /// </summary>
        event PlanPageExpenseEntrySelectedEventHandler ExpenseEntrySelected;


        /// <summary>
        /// Manages the currently selected tab of the page
        /// </summary>
        public PlanPageTab ActiveTab { get; set; }
        /// <summary>
        /// Stores items which are used to build the income chart
        /// </summary>
        public ICollection<PlanPageIncomeChartEntry> IncomeChartItems { get; }
        /// <summary>
        /// Stores items which are used to build expenses chart
        /// </summary>
        public ICollection<PlanPageExpenseChartEntry> ExpensesChartItems { get; }
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