using System;
using System.ComponentModel;
using System.Globalization;

namespace ApplicationProject.Views.DatedPageView
{
    public class DateRangeType : INotifyPropertyChanged, ICultureDependentData
    {
        protected const string DisplayNameMonthKey = "DATERANGE_TYPENAME_MONTH";
        protected const string DisplayNameYearKey = "DATERANGE_TYPENAME_YEAR";

        public enum RangeType
        {
            MONTH,
            YEAR
        }

        public string DisplayName
        {
            get => Type switch
            {
                RangeType.MONTH => DisplayNameMonthKey,
                RangeType.YEAR => DisplayNameYearKey,
                _ => throw new ArgumentOutOfRangeException(nameof(Type))
            };
        }

        public RangeType Type { get; set; }

        public CultureInfo CurrentCulture
        {
            get => m_CurrentCulture;
            set
            {
                m_CurrentCulture = value ?? System.Threading.Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture ?? CultureInfo.InvariantCulture;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
            }
        }
        private CultureInfo m_CurrentCulture;

        public DateRangeType()
        {
            Type = 0;
            CurrentCulture = null;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region ICultureDependentData
        public void OnCultureChanged(CultureInfo newCulture)
        {
            CurrentCulture = newCulture;
        }
        #endregion
    }
}
