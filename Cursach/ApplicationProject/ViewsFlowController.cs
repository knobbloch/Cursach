using System;

using ApplicationProjectViews;

using ApplicationProjectViews.AnalysisPageView;
using ApplicationProjectViews.DatedPageView;
using ApplicationProjectViews.InterPageView;
using ApplicationProjectViews.PlanPageView;
using ApplicationProjectViews.AddExpensePageView;
using ApplicationProjectViews.AddExpenseCategoryPageView;
using ApplicationProjectViews.AddIncomePageView;
using ApplicationProjectViews.AddIncomeCategoryPageView;
using ApplicationProjectViews.AddBankAccountPageView;

using ApplicationProject.UserControls.AnalysisPageView;
using ApplicationProject.UserControls.DatedPageView;
using ApplicationProject.UserControls.InterPageView;
using ApplicationProject.UserControls.PlanPageView;
using ApplicationProject.UserControls.AddExpensePageView;
using ApplicationProject.UserControls.AddExpenseCategoryPageView;
using ApplicationProject.UserControls.AddIncomePageView;
using ApplicationProject.UserControls.AddIncomeCategoryPageView;
using ApplicationProject.UserControls.AddBankAccountPageView;

namespace ApplicationProject
{
    public sealed class ViewsFlowController
    {
        private const string AnalysisPageNameKey = "PAGE_ANALYSIS_NAME";
        private const string PlanPageNameKey = "PAGE_PLAN_NAME";
        private const string AddExpenseCategoryPageNameKey = "PAGE_ADDEXPENSECATEGORY_NAME";
        private const string AddIncomeCategoryPageNameKey = "PAGE_ADDINCOMECATEGORY_NAME";

        public IInterPageView IInterPageViewInstance => InterPageViewInstance;
        public IDatedPageView IDatedPageViewInstance => DatedPageViewInstance;
        public IAnalysisPageView IAnalysisPageViewInstance => AnalysisPageViewInstance;
        public IPlanPageView IPlanPageViewInstance => PlanPageViewInstance;
        public IAddExpensePageView IAddExpensePageViewInstance => AddExpensePageViewInstance;
        public IAddExpenseCategoryPageView IAddExpenseCategoryPageViewInstance => AddExpenseCategoryPageViewInstance;
        public IAddIncomePageView IAddIncomePageViewInstance => AddIncomePageViewInstance;
        public IAddIncomeCategoryPageView IAddIncomeCategoryPageViewInstance => AddIncomeCategoryPageViewInstance;
        public IAddBankAccountPageView IAddBankAccountPageViewInstance => AddBankAccountPageViewInstance;
        public IViewPresenter ViewRoot { get; }

        private InterPageView InterPageViewInstance { get; }
        private DatedPageView DatedPageViewInstance { get; }
        private AnalysisPageView AnalysisPageViewInstance { get; }
        private PlanPageView PlanPageViewInstance { get; }
        private AddExpensePageView AddExpensePageViewInstance { get; }
        private AddExpenseCategoryPageView AddExpenseCategoryPageViewInstance { get; }
        private AddIncomePageView AddIncomePageViewInstance { get; }
        private AddIncomeCategoryPageView AddIncomeCategoryPageViewInstance { get; }
        private AddBankAccountPageView AddBankAccountPageViewInstance { get; }

        private IBaseView PreviousView { get; set; }
        private string PreviousViewName { get; set; }

        public ViewsFlowController(IViewPresenter viewRoot)
        {
            ViewRoot = viewRoot;

            //Initialize InterPageView
            InterPageViewInstance = new();
            IInterPageViewInstance.CategorySelectedAction += IInterPageViewInstance_CategorySelectedAction;
            IInterPageViewInstance.AddBankAccountAction += IInterPageViewInstance_AddBankAccountAction;

            DatedPageViewInstance = new();
            DatedPageViewInstance.PageNameTextKey = AnalysisPageNameKey;

            AnalysisPageViewInstance = new();
            AnalysisPageViewInstance.AddExpenseAction += AnalysisPageViewInstance_AddExpenseAction;
            AnalysisPageViewInstance.AddExpenseCategoryAction += AnalysisPageViewInstance_AddExpenseCategoryAction;
            AnalysisPageViewInstance.AddIncomeAction += AnalysisPageViewInstance_AddIncomeAction;
            AnalysisPageViewInstance.AddIncomeCategoryAction += AnalysisPageViewInstance_AddIncomeCategoryAction;

            PlanPageViewInstance = new();
            PlanPageViewInstance.AddExpenseCategoryAction += PlanPageViewInstance_AddExpenseCategoryAction;
            PlanPageViewInstance.AddIncomeCategoryAction += PlanPageViewInstance_AddIncomeCategoryAction;

            AddExpensePageViewInstance = new();
            AddExpensePageViewInstance.AddActionPost += AddExpensePageViewInstance_AddActionPost;
            AddExpensePageViewInstance.ExitAction += AddPageViewInstance_ExitAction;

            AddExpenseCategoryPageViewInstance = new();
            AddExpenseCategoryPageViewInstance.AddActionPost += AddExpenseCategoryPageViewInstance_AddActionPost;
            AddExpenseCategoryPageViewInstance.ExitAction += AddCategoryPageViewInstance_ExitAction;

            AddIncomePageViewInstance = new();
            AddIncomePageViewInstance.AddActionPost += AddIncomePageViewInstance_AddActionPost;
            AddIncomePageViewInstance.ExitAction += AddPageViewInstance_ExitAction;

            AddIncomeCategoryPageViewInstance = new();
            AddIncomeCategoryPageViewInstance.AddActionPost += AddIncomeCategoryPageViewInstance_AddActionPost;
            AddIncomeCategoryPageViewInstance.ExitAction += AddCategoryPageViewInstance_ExitAction;

            AddBankAccountPageViewInstance = new();
            AddBankAccountPageViewInstance.AddActionPost += AddBankAccountPageViewInstance_AddActionPost;
            AddBankAccountPageViewInstance.ExitAction += AddPageViewInstance_ExitAction;
        }

        private void IInterPageViewInstance_AddBankAccountAction(object sender, EventArgs e)
        {
            _ = InterPageViewInstance.Present(IAddBankAccountPageViewInstance);
        }

        private void AddBankAccountPageViewInstance_AddActionPost(object sender, EventArgs e)
        {
            if ((AddBankAccountPageViewInstance.CurrencyAmountError ??
                AddBankAccountPageViewInstance.AccountNameError) == null)
            {
                AddPageViewInstance_ExitAction(sender, e);
            }
        }

        private void PlanPageViewInstance_AddIncomeCategoryAction(object sender, EventArgs e)
        {
            PreviousView = IPlanPageViewInstance;
            PreviousViewName = PlanPageNameKey;

            _ = DatedPageViewInstance.Present(IAddIncomeCategoryPageViewInstance);
            DatedPageViewInstance.PageNameTextKey = AddIncomeCategoryPageNameKey;
        }

        private void AnalysisPageViewInstance_AddIncomeCategoryAction(object sender, EventArgs e)
        {
            PreviousView = IAnalysisPageViewInstance;
            PreviousViewName = AnalysisPageNameKey;

            _ = DatedPageViewInstance.Present(IAddIncomeCategoryPageViewInstance);
            DatedPageViewInstance.PageNameTextKey = AddIncomeCategoryPageNameKey;
        }

        private void AnalysisPageViewInstance_AddIncomeAction(object sender, EventArgs e)
        {
            _ = InterPageViewInstance.Present(IAddIncomePageViewInstance);
        }

        private void AddIncomePageViewInstance_AddActionPost(object sender, EventArgs e)
        {
            if ((AddIncomePageViewInstance.CurrencyAmountError ??
                AddIncomePageViewInstance.IncomeNameError) == null)
            {
                AddPageViewInstance_ExitAction(sender, e);
            }
        }

        private void AddIncomeCategoryPageViewInstance_AddActionPost(object sender, EventArgs e)
        {
            if ((AddIncomeCategoryPageViewInstance.CurrencyAmountError ??
                AddIncomeCategoryPageViewInstance.CategoryNameError ??
                AddIncomeCategoryPageViewInstance.CategoryImagePathError) == null)
            {
                AddCategoryPageViewInstance_ExitAction(sender, e);
            }
        }

        private void AddCategoryPageViewInstance_ExitAction(object sender, EventArgs e)
        {
            _ = DatedPageViewInstance.Present(PreviousView);
            DatedPageViewInstance.PageNameTextKey = PreviousViewName;

            PreviousView = null;
            PreviousViewName = null;
        }

        private void AddExpenseCategoryPageViewInstance_AddActionPost(object sender, EventArgs e)
        {
            if ((AddExpenseCategoryPageViewInstance.CurrencyAmountError ??
                AddExpenseCategoryPageViewInstance.CategoryNameError ??
                AddExpenseCategoryPageViewInstance.CategoryImagePathError) == null)
            {
                AddCategoryPageViewInstance_ExitAction(sender, e);
            }
        }

        private void AddPageViewInstance_ExitAction(object sender, EventArgs e)
        {
            _ = InterPageViewInstance.Present(IDatedPageViewInstance);
        }

        private void AddExpensePageViewInstance_AddActionPost(object sender, EventArgs e)
        {
            if ((AddExpensePageViewInstance.CurrencyAmountError ??
                AddExpensePageViewInstance.ExpenseNameError) == null)
            {
                AddPageViewInstance_ExitAction(sender, e);
            }
        }

        private void PlanPageViewInstance_AddExpenseCategoryAction(object sender, EventArgs e)
        {
            PreviousView = IPlanPageViewInstance;
            PreviousViewName = PlanPageNameKey;

            _ = DatedPageViewInstance.Present(IAddExpenseCategoryPageViewInstance);
            DatedPageViewInstance.PageNameTextKey = AddExpenseCategoryPageNameKey;
        }

        private void AnalysisPageViewInstance_AddExpenseCategoryAction(object sender, EventArgs e)
        {
            PreviousView = IAnalysisPageViewInstance;
            PreviousViewName = AnalysisPageNameKey;

            _ = DatedPageViewInstance.Present(IAddExpenseCategoryPageViewInstance);
            DatedPageViewInstance.PageNameTextKey = AddExpenseCategoryPageNameKey;
        }

        private void AnalysisPageViewInstance_AddExpenseAction(object sender, EventArgs e)
        {
            _ = InterPageViewInstance.Present(IAddExpensePageViewInstance);
        }

        public bool Link()
        {
            return ViewRoot.Present(IInterPageViewInstance) &&
                   InterPageViewInstance.Present(IDatedPageViewInstance) &&
                   DatedPageViewInstance.Present(IAnalysisPageViewInstance);
        }

        private void IInterPageViewInstance_CategorySelectedAction(object source, CategorySelectedEventArgs e)
        {
            switch (e.Category)
            {
                case CategorySelectedEventArgs.CategoryType.Analysis:
                    {
                        _ = DatedPageViewInstance.Present(IAnalysisPageViewInstance);
                        DatedPageViewInstance.PageNameTextKey = AnalysisPageNameKey;
                        if (InterPageViewInstance.PresentedView != IDatedPageViewInstance)
                            _ = InterPageViewInstance.Present(IDatedPageViewInstance);
                        break;
                    }
                case CategorySelectedEventArgs.CategoryType.Plan:
                    {
                        _ = DatedPageViewInstance.Present(IPlanPageViewInstance);
                        DatedPageViewInstance.PageNameTextKey = PlanPageNameKey;
                        if (InterPageViewInstance.PresentedView != IDatedPageViewInstance)
                            _ = InterPageViewInstance.Present(IDatedPageViewInstance);
                        break;
                    }
                case CategorySelectedEventArgs.CategoryType.NewEntry:
                    {
                        if (InterPageViewInstance.PresentedView != IAddExpensePageViewInstance)
                            _ = InterPageViewInstance.Present(IAddExpensePageViewInstance);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(e.Category));
            }
        }
    }
}
