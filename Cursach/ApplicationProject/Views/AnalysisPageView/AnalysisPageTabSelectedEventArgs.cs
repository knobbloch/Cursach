using System;

namespace ApplicationProject.Views.AnalysisPageView
{
    public class AnalysisPageTabSelectedEventArgs : EventArgs
    {
        public IAnalysisPageView.AnalysisPageTab Tab { get; }

        public AnalysisPageTabSelectedEventArgs(IAnalysisPageView.AnalysisPageTab tab)
        {
            Tab = tab;
        }
    }
}
