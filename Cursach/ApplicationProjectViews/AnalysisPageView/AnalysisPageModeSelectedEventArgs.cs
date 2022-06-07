using System;

namespace ApplicationProjectViews.AnalysisPageView
{
    public delegate void AnalysisPageModeSelectedEventHandler(object source, AnalysisPageModeSelectedEventArgs e);

    public class AnalysisPageModeSelectedEventArgs : EventArgs
    {
        public IAnalysisPageView.AnalysisPageMode Mode { get; init; }

        public AnalysisPageModeSelectedEventArgs(IAnalysisPageView.AnalysisPageMode mode) : base()
        {
            Mode = mode;
        }
    }
}
