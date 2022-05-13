using System;
using System.ComponentModel;

namespace ApplicationProject.Views.PlanPageView
{
    public class PlanPageExpenseChartEntry : INotifyPropertyChanged
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
        /// The path to the image displayed by the text
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public string ImagePath
        {
            get => m_ImagePath;
            set
            {
                m_ImagePath = value ?? throw new ArgumentNullException(nameof(ImagePath));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePath)));
            }
        }
        private string m_ImagePath;

        public PlanPageExpenseChartEntry() : this("", 0) { }
        public PlanPageExpenseChartEntry(string path, double value)
        {
            Value = value;
            ImagePath = path;
        }
    }
}
