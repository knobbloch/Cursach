using System;
using System.ComponentModel;

namespace ApplicationProject.Views.PlanPageView
{
    public class PlanPageIncomeChartEntry : INotifyPropertyChanged
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
        public string Title
        {
            get => m_Title;
            set
            {
                m_Title = value ?? throw new ArgumentNullException(nameof(Title));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }
        private string m_Title;

        public PlanPageIncomeChartEntry() : this("", 0) { }
        public PlanPageIncomeChartEntry(string title, double value)
        {
            Value = value;
            Title = title;
        }
    }
}
