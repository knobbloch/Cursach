﻿using System;
using System.ComponentModel;

namespace ApplicationProjectViews.PlanPageView
{
    public class PlanPageIncomeEntry : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The displayed real value of this entry
        /// </summary>
        public double RealValue
        {
            get => m_RealValue;
            set
            {
                m_RealValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RealValue)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Difference)));
            }
        }
        private double m_RealValue;

        /// <summary>
        /// The displayed planned value of this entry
        /// </summary>
        public double PlannedValue
        {
            get => m_PlannedValue;
            set
            {
                m_PlannedValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlannedValue)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Difference)));
            }
        }
        private double m_PlannedValue;

        /// <summary>
        /// The difference between real and planned values
        /// </summary>
        public double Difference => RealValue - PlannedValue;

        /// <summary>
        /// The currency identifier to append to the values
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
        /// The text to display as the entry's title
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

        public PlanPageIncomeEntry() : this("", "", 0, 0) { }
        public PlanPageIncomeEntry(string title, string currencyIdentifier, double rValue, double pValue)
        {
            RealValue = rValue;
            PlannedValue = pValue;
            Title = title;
            CurrencyIdentifier = currencyIdentifier;
        }
    }
}
