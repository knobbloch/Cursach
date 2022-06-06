using System;

namespace ApplicationProjectViews.PlanPageView
{
    public delegate void PlanPageIncomeEntrySelectedEventHandler(object source, PlanPageIncomeEntrySelectedEventArgs e);

    public class PlanPageIncomeEntrySelectedEventArgs : EventArgs
    {
        public PlanPageIncomeEntry Entry
        {
            get => m_Entry;
            init => m_Entry = value ?? throw new ArgumentNullException(nameof(Entry));
        }
        private PlanPageIncomeEntry m_Entry;

        public PlanPageIncomeEntrySelectedEventArgs(PlanPageIncomeEntry entry) : base()
        {
            Entry = entry;
        }
    }
}
