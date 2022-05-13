using System;
using System.ComponentModel;

namespace ApplicationProject.Views.AnalysisPageView
{
    public class AnalysisPageExpenseChartEntry : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The displayed value of this entry
        /// </summary>
        public double Value
        {
            get => m_Value;
            set
            {
                m_Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
        private double m_Value;

        /// <summary>
        /// The text to display as the chart entry's title
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public string PeriodTitle
        {
            get => m_PeriodTitle;
            set
            {
                m_PeriodTitle = value ?? throw new ArgumentNullException(nameof(PeriodTitle));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PeriodTitle)));
            }
        }
        private string m_PeriodTitle;

        public AnalysisPageExpenseChartEntry() : this("", 0) { }
        public AnalysisPageExpenseChartEntry(string title, double value)
        {
            Value = value;
            PeriodTitle = title;
        }
    }
}
