using System;

namespace ApplicationProject.Views.PlanPageView
{
    public class PlanPageTabSelectedEventArgs : EventArgs
    {
        public IPlanPageView.PlanPageTab Tab { get; }

        public PlanPageTabSelectedEventArgs(IPlanPageView.PlanPageTab tab)
        {
            Tab = tab;
        }
    }
}
