using System;

namespace ApplicationProject.Views.AnalysisPageView
{
    public delegate void AnalysisPageExpenseEntrySelectedEventHandler(object source, AnalysisPageExpenseEntrySelectedEventArgs e);

    public class AnalysisPageExpenseEntrySelectedEventArgs : EventArgs
    {
        public AnalysisPageExpenseEntry Entry { get; }

        public AnalysisPageExpenseEntrySelectedEventArgs(AnalysisPageExpenseEntry entry) : base()
        {
            Entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }
    }
}
