using System;

namespace ApplicationProjectViews.AnalysisPageView
{
    public class AnalysisPageIncomeDayEntry
    {
        /// <summary>
        /// The displayed value of this entry
        /// </summary>
        public double Value { get; init; }

        /// <summary>
        /// The text to display as the chart entry's title
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public string PeriodTitle
        {
            get => m_PeriodTitle;
            init => m_PeriodTitle = value ?? throw new ArgumentNullException(nameof(PeriodTitle));
        }
        private string m_PeriodTitle;

        public AnalysisPageIncomeDayEntry()
        {
            Value = 0;
            PeriodTitle = "";
        }
    }
}
