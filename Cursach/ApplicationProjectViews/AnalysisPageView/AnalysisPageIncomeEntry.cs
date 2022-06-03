using System;
using System.ComponentModel;

namespace ApplicationProjectViews.AnalysisPageView
{
    public class AnalysisPageIncomeEntry : INotifyPropertyChanged
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
        /// The currency identifier to append to the value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public string CurrencyIdentifier
        {
            get => m_CurrencyIdentifier;
            set
            {
                m_CurrencyIdentifier = value ?? throw new ArgumentNullException(nameof(CurrencyIdentifier));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyIdentifier)));
            }
        }
        private string m_CurrencyIdentifier;

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

        public AnalysisPageIncomeEntry() : this("", "", 0) { }
        public AnalysisPageIncomeEntry(string title, string currencyIdentifier, double value)
        {
            Value = value;
            Title = title;
            CurrencyIdentifier = currencyIdentifier;
        }
    }
}
