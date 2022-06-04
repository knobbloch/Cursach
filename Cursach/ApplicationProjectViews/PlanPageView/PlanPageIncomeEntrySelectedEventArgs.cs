using System;

namespace ApplicationProjectViews.PlanPageView
{
    public delegate void PlanPageIncomeEntrySelectedEventHandler(object source, PlanPageIncomeEntrySelectedEventArgs e);

    public class PlanPageIncomeEntrySelectedEventArgs : EventArgs
    {
        public PlanPageIncomeEntry Entry { get; }

        public PlanPageIncomeEntrySelectedEventArgs(PlanPageIncomeEntry entry) : base()
        {
            Entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }
    }
}
