using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ApplicationProject.Views.AnalysisPageView
{
    public class AnalysisPageChartIncomeEntry : INotifyPropertyChanged
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
        public string PeriodTitle
        {
            get => m_PeriodTitle;
            set
            {
                m_PeriodTitle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PeriodTitle)));
            }
        }
        private string m_PeriodTitle;

        public AnalysisPageChartIncomeEntry() : this("", 0) { }
        public AnalysisPageChartIncomeEntry(string title, double value)
        {
            m_Value = value;
            m_PeriodTitle = title ?? throw new ArgumentNullException(nameof(title));
        }
    }
}
