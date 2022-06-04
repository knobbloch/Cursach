using System;

namespace ApplicationProjectViews.AnalysisPageView
{
    public delegate void AnalysisPageIncomeEntrySelectedEventHandler(object source, AnalysisPageIncomeEntrySelectedEventArgs e);

    public class AnalysisPageIncomeEntrySelectedEventArgs : EventArgs
    {
        public AnalysisPageIncomeEntry Entry { get; }

        public AnalysisPageIncomeEntrySelectedEventArgs(AnalysisPageIncomeEntry entry) : base()
        {
            Entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }
    }
}
