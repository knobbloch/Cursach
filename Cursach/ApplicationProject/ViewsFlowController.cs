using System;

using ApplicationProjectViews;

using ApplicationProjectViews.AnalysisPageView;
using ApplicationProjectViews.DatedPageView;
using ApplicationProjectViews.InterPageView;
using ApplicationProjectViews.PlanPageView;
using ApplicationProjectViews.AddExpensePageView;
using ApplicationProjectViews.AddExpenseCategoryPageView;

using ApplicationProject.UserControls.AnalysisPageView;
using ApplicationProject.UserControls.DatedPageView;
using ApplicationProject.UserControls.InterPageView;
using ApplicationProject.UserControls.PlanPageView;
using ApplicationProject.UserControls.AddExpensePageView;
using ApplicationProject.UserControls.AddExpenseCategoryPageView;

namespace ApplicationProject
{
    public sealed class ViewsFlowController
    {
        private const string AnalysisPageNameKey = "PAGE_ANALYSIS_NAME";
        private const string PlanPageNameKey = "PAGE_PLAN_NAME";
        private const string AddExpensePageNameKey = "PAGE_ADDEXPENSE_NAME";
        private const string AddExpenseCategoryPageNameKey = "PAGE_ADDEXPENSECATEGORY_NAME";

        public IInterPageView IInterPageViewInstance => InterPageViewInstance;
        public IDatedPageView IDatedPageViewInstance => DatedPageViewInstance;
        public IAnalysisPageView IAnalysisPageViewInstance => AnalysisPageViewInstance;
        public IPlanPageView IPlanPageViewInstance => PlanPageViewInstance;
        public IAddExpensePageView IAddExpensePageViewInstance => AddExpensePageViewInstance;
        public IAddExpenseCategoryPageView IAddExpenseCategoryPageViewInstance => AddExpenseCategoryPageViewInstance;
        public IViewPresenter ViewRoot { get; }

        private InterPageView InterPageViewInstance { get; }
        private DatedPageView DatedPageViewInstance { get; }
        private AnalysisPageView AnalysisPageViewInstance { get; }
        private PlanPageView PlanPageViewInstance { get; }
        private AddExpensePageView AddExpensePageViewInstance { get; }
        private AddExpenseCategoryPageView AddExpenseCategoryPageViewInstance { get; }

        private IBaseView PreviousView { get; set; }

        public ViewsFlowController(IViewPresenter viewRoot)
        {
            ViewRoot = viewRoot;

            //Initialize InterPageView
            InterPageViewInstance = new();
            IInterPageViewInstance.CategorySelectedAction += IInterPageViewInstance_CategorySelectedAction;

            DatedPageViewInstance = new();
            DatedPageViewInstance.PageNameTextKey = AnalysisPageNameKey;

            AnalysisPageViewInstance = new();
            AnalysisPageViewInstance.AddExpenseAction += AnalysisPageViewInstance_AddExpenseAction;
            AnalysisPageViewInstance.AddExpenseCategoryAction += PageViewInstance_AddExpenseCategoryAction;

            PlanPageViewInstance = new();
            PlanPageViewInstance.AddExpenseCategoryAction += PageViewInstance_AddExpenseCategoryAction;

            AddExpensePageViewInstance = new();
            AddExpensePageViewInstance.AddActionPost += AddExpensePageViewInstance_AddActionPost;
            AddExpensePageViewInstance.ExitAction += AddPageViewInstance_ExitAction;

            AddExpenseCategoryPageViewInstance = new();
            AddExpenseCategoryPageViewInstance.AddActionPost += AddExpenseCategoryPageViewInstance_AddActionPost;
            AddExpenseCategoryPageViewInstance.ExitAction += AddPageViewInstance_ExitAction;
        }

        private void PageViewInstance_AddExpenseCategoryAction(object sender, EventArgs e)
        {
            PreviousView = (IBaseView)sender;
            _ = DatedPageViewInstance.Present(IAddExpenseCategoryPageViewInstance);
            DatedPageViewInstance.PageNameTextKey = AddExpenseCategoryPageNameKey;
        }

        private void AddExpenseCategoryPageViewInstance_AddActionPost(object sender, EventArgs e)
        {
            if ((IAddExpenseCategoryPageViewInstance.CategoryImagePathError ??
                IAddExpenseCategoryPageViewInstance.CategoryNameError ??
                IAddExpenseCategoryPageViewInstance.CurrencyAmountError) == null)
            {
                AddPageViewInstance_ExitAction(sender, e);
            }
        }

        private void AnalysisPageViewInstance_AddExpenseAction(object sender, EventArgs e)
        {
            PreviousView = AnalysisPageViewInstance;
            _ = DatedPageViewInstance.Present(IAddExpensePageViewInstance);
            DatedPageViewInstance.PageNameTextKey = AddExpensePageNameKey;
        }

        private void AddPageViewInstance_ExitAction(object sender, EventArgs e)
        {
            _ = DatedPageViewInstance.Present(PreviousView);

            if (PreviousView == AnalysisPageViewInstance)
                DatedPageViewInstance.PageNameTextKey = AnalysisPageNameKey;
            else if (PreviousView == PlanPageViewInstance)
                DatedPageViewInstance.PageNameTextKey = PlanPageNameKey;
            else
                throw new ArgumentException(nameof(PreviousView));

            PreviousView = null;
        }

        private void AddExpensePageViewInstance_AddActionPost(object sender, EventArgs e)
        {
            if ((AddExpensePageViewInstance.ExpenseNameError ??
                AddExpensePageViewInstance.CurrencyAmountError) == null)
            {
                AddPageViewInstance_ExitAction(sender, e);
            }
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
                        break;
                    }
                case CategorySelectedEventArgs.CategoryType.Plan:
                    {
                        _ = DatedPageViewInstance.Present(IPlanPageViewInstance);
                        DatedPageViewInstance.PageNameTextKey = PlanPageNameKey;
                        break;
                    }
                case CategorySelectedEventArgs.CategoryType.NewEntry:
                    {
                        if (DatedPageViewInstance.PresentedView != AddExpensePageViewInstance)
                        {
                            if (DatedPageViewInstance.PresentedView == AnalysisPageViewInstance || DatedPageViewInstance.PresentedView == PlanPageViewInstance)
                                PreviousView = DatedPageViewInstance.PresentedView;
                            
                            _ = DatedPageViewInstance.Present(IAddExpensePageViewInstance);
                            DatedPageViewInstance.PageNameTextKey = AddExpensePageNameKey;
                        }
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(e.Category));
            }
        }
    }
}
