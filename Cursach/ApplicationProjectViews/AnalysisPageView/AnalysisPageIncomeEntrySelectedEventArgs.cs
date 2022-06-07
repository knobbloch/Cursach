using System;

namespace ApplicationProjectViews.AnalysisPageView
{
    public delegate void AnalysisPageIncomeEntrySelectedEventHandler(object source, AnalysisPageIncomeEntrySelectedEventArgs e);

    public class AnalysisPageIncomeEntrySelectedEventArgs : EventArgs
    {
        public AnalysisPageIncomeEntry Entry
        {
            get => m_Entry;
            init => m_Entry = value ?? throw new ArgumentNullException(nameof(Entry));
        }
        private AnalysisPageIncomeEntry m_Entry;

        public AnalysisPageIncomeEntrySelectedEventArgs(AnalysisPageIncomeEntry entry) : base()
        {
            Entry = entry;
        }
    }
}
