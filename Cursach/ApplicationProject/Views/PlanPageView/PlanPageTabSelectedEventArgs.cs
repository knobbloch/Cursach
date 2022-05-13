using System;

namespace ApplicationProject.Views.PlanPageView
{
    public delegate void PlanPageTabSelectedEventHandler(object source, PlanPageTabSelectedEventArgs e);

    public class PlanPageTabSelectedEventArgs : EventArgs
    {
        public IPlanPageView.PlanPageTab Tab { get; }

        public PlanPageTabSelectedEventArgs(IPlanPageView.PlanPageTab tab) : base()
        {
            Tab = tab;
        }
    }
}
