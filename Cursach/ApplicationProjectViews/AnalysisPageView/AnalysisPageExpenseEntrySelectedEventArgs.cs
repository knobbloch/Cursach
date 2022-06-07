using System;

namespace ApplicationProjectViews.AnalysisPageView
{
    public delegate void AnalysisPageExpenseEntrySelectedEventHandler(object source, AnalysisPageExpenseEntrySelectedEventArgs e);

    public class AnalysisPageExpenseEntrySelectedEventArgs : EventArgs
    {
        public AnalysisPageExpenseEntry Entry
        {
            get => m_Entry;
            init => m_Entry = value ?? throw new ArgumentNullException(nameof(Entry));
        }
        private AnalysisPageExpenseEntry m_Entry;

        public AnalysisPageExpenseEntrySelectedEventArgs(AnalysisPageExpenseEntry entry) : base()
        {
            Entry = entry;
        }
    }
}
