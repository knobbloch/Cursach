using System;

namespace ApplicationProjectViews.PlanPageView
{
    public delegate void PlanPageModeSelectedEventHandler(object source, PlanPageModeSelectedEventArgs e);

    public class PlanPageModeSelectedEventArgs : EventArgs
    {
        public IPlanPageView.PlanPageMode Mode { get; init; }

        public PlanPageModeSelectedEventArgs(IPlanPageView.PlanPageMode mode) : base()
        {
            Mode = mode;
        }
    }
}
