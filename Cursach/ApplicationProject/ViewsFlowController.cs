using System;

using ApplicationProject.Views;

using ApplicationProject.Views.AnalysisPageView;
using ApplicationProject.Views.DatedPageView;
using ApplicationProject.Views.InterPageView;
using ApplicationProject.Views.PlanPageView;

using ApplicationProject.UserControls.AnalysisPageView;
using ApplicationProject.UserControls.DatedPageView;
using ApplicationProject.UserControls.InterPageView;
using ApplicationProject.UserControls.PlanPageView;

namespace ApplicationProject
{
    public sealed class ViewsFlowController
    {
        private const string AnalysisPageNameKey = "PAGE_ANALYSIS_NAME";
        private const string PlanPageNameKey = "PAGE_PLAN_NAME";

        public IInterPageView IInterPageViewInstance => InterPageViewInstance;
        public IDatedPageView IDatedPageViewInstance => DatedPageViewInstance;
        public IAnalysisPageView IAnalysisPageViewInstance => AnalysisPageViewInstance;
        public IPlanPageView IPlanPageViewInstance => PlanPageViewInstance;
        public IViewPresenter ViewRoot { get; }

        private InterPageView InterPageViewInstance { get; }
        private DatedPageView DatedPageViewInstance { get; }
        private AnalysisPageView AnalysisPageViewInstance { get; }
        private PlanPageView PlanPageViewInstance { get; }

        public ViewsFlowController(IViewPresenter viewRoot)
        {
            ViewRoot = viewRoot;

            //Initialize InterPageView
            InterPageViewInstance = new InterPageView();
            IInterPageViewInstance.CategorySelectedAction += IInterPageViewInstance_CategorySelectedAction;

            DatedPageViewInstance = new DatedPageView();
            DatedPageViewInstance.PageNameTextKey = AnalysisPageNameKey;
            
            AnalysisPageViewInstance = new AnalysisPageView();
            
            PlanPageViewInstance = new PlanPageView();

        }

        public bool Link()
        {
            return ViewRoot.Present(IInterPageViewInstance) &&
                   InterPageViewInstance.Present(IDatedPageViewInstance) &&
                   DatedPageViewInstance.Present(IAnalysisPageViewInstance);
        }

        private void IInterPageViewInstance_CategorySelectedAction(object source, CategorySelectedEventArgs e)
        {
            _ = DatedPageViewInstance.Present(e.Category switch
            {
                CategorySelectedEventArgs.CategoryType.Plan => IPlanPageViewInstance,
                CategorySelectedEventArgs.CategoryType.Analysis => IAnalysisPageViewInstance,
                _ => throw new ArgumentOutOfRangeException(nameof(e.Category))
            });

            DatedPageViewInstance.PageNameTextKey = e.Category switch
            {
                CategorySelectedEventArgs.CategoryType.Plan => PlanPageNameKey,
                CategorySelectedEventArgs.CategoryType.Analysis => AnalysisPageNameKey,
                _ => throw new ArgumentOutOfRangeException(nameof(e.Category))
            };
        }
    }
}
