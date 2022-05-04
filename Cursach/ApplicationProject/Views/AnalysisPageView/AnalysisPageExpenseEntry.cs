﻿using System;
using System.ComponentModel;

namespace ApplicationProject.Views.AnalysisPageView
{
    public class AnalysisPageExpenseEntry : INotifyPropertyChanged
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

        public string CurrencyIdentifier
        {
            get => m_CurrencyIdentifier;
            set
            {
                m_CurrencyIdentifier = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyIdentifier)));
            }
        }
        private string m_CurrencyIdentifier;

        /// <summary>
        /// The text to display as the chart entry's title
        /// </summary>
        public string Title
        {
            get => m_Title;
            set
            {
                m_Title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }
        private string m_Title;

        public string ImagePath
        {
            get => m_ImagePath;
            set
            {
                m_ImagePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePath)));
            }
        }
        private string m_ImagePath;

        public AnalysisPageExpenseEntry(string title, string currencyIdentifier, string imagePath, double value)
        {
            m_Value = value;
            m_Title = title ?? throw new ArgumentNullException(nameof(title));
            m_CurrencyIdentifier = currencyIdentifier ?? throw new ArgumentNullException(nameof(currencyIdentifier));
            m_ImagePath = imagePath ?? throw new ArgumentNullException(nameof(currencyIdentifier));
        }
    }
}