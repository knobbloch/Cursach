using System;

namespace ApplicationProject.Views.AnalysisPageView
{
    public delegate void AnalysisPageTabSelectedEventHandler(object source, AnalysisPageTabSelectedEventArgs e);

    public class AnalysisPageTabSelectedEventArgs : EventArgs
    {
        public IAnalysisPageView.AnalysisPageTab Tab { get; }

        public AnalysisPageTabSelectedEventArgs(IAnalysisPageView.AnalysisPageTab tab) : base()
        {
            Tab = tab;
        }
    }
}
