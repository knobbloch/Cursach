using System;
using System.Collections.Generic;

namespace ApplicationProject.Views.AnalysisPageView
{
    public interface IAnalysisPageView : IBaseView
    {
        enum AnalysisPageTab
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
        /// Sets the key for the name of the "Value" header in the expenes table
        /// </summary>
        public string ExpensesTableValueHeaderKey { set; }
        /// <summary>
        /// Sets the key for the name of the "Name" header in the income table
        /// </summary>
        public string IncomeTableNameHeaderKey { set; }
        /// <summary>
        /// Sets the key for the name of the "Value" header in the expenes table
        /// </summary>
        public string IncomeTableValueHeaderKey { set; }
        /// <summary>
        /// Sets the key for the text of the "add expense" tab
        /// </summary>
        public string AddExpenseTextKey { set; }
        /// <summary>
        /// Sets the key for the text of the "add expense category" tab
        /// </summary>
        public string AddExpenseCategoryTextKey { set; }
        /// <summary>
        /// Sets the key for the text of the "create expenses report" tab
        /// </summary>
        public string CreateExpensesReportTextKey { set; }
        /// <summary>
        /// Sets the key for the text of the "add income" tab
        /// </summary>
        public string AddIncomeTextKey { set; }
        /// <summary>
        /// Sets the key for the text of the "create income report" tab
        /// </summary>
        public string CreateIncomeReportTextKey { set; }

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
        public ICollection<AnalysisPageIncomeChartEntry> IncomeChartItems { get; }
        /// <summary>
        /// Stores items which are used to build expenses chart
        /// </summary>
        public ICollection<AnalysisPageExpenseChartEntry> ExpensesChartItems { get; }
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