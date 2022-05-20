using System;

namespace ApplicationProject.Views.PlanPageView
{
    public delegate void PlanPageTabSelectedEventHandler(object source, PlanPageTabSelectedEventArgs e);

    public class PlanPageTabSelectedEventArgs : EventArgs
    {
        public IPlanPageView.PlanPageMode Mode { get; }

        public PlanPageTabSelectedEventArgs(IPlanPageView.PlanPageMode mode) : base()
        {
            Mode = mode;
        }
    }
}
