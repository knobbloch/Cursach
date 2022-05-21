using System;

namespace ApplicationProject.Views.AnalysisPageView
{
    public delegate void AnalysisPageModeSelectedEventHandler(object source, AnalysisPageModeSelectedEventArgs e);

    public class AnalysisPageModeSelectedEventArgs : EventArgs
    {
        public IAnalysisPageView.AnalysisPageMode Mode { get; }

        public AnalysisPageModeSelectedEventArgs(IAnalysisPageView.AnalysisPageMode mode) : base()
        {
            Mode = mode;
        }
    }
}
