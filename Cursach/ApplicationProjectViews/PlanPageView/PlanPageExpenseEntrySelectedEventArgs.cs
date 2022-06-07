using System;

namespace ApplicationProjectViews.PlanPageView
{
    public delegate void PlanPageExpenseEntrySelectedEventHandler(object source, PlanPageExpenseEntrySelectedEventArgs e);

    public class PlanPageExpenseEntrySelectedEventArgs : EventArgs
    {
        public PlanPageExpenseEntry Entry
        {
            get => m_Entry;
            init => m_Entry = value ?? throw new ArgumentNullException(nameof(Entry));
        }
        private PlanPageExpenseEntry m_Entry;

        public PlanPageExpenseEntrySelectedEventArgs(PlanPageExpenseEntry entry) : base()
        {
            Entry = entry;
        }
    }
}
