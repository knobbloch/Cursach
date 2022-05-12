using System;

namespace ApplicationProject.Views.PlanPageView
{
    public delegate void PlanPageExpenseEntrySelectedEventHandler(object source, PlanPageExpenseEntrySelectedEventArgs e);

    public class PlanPageExpenseEntrySelectedEventArgs : EventArgs
    {
        public PlanPageExpenseEntry Entry { get; }

        public PlanPageExpenseEntrySelectedEventArgs(PlanPageExpenseEntry entry) : base()
        {
            Entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }
    }
}
