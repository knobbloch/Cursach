using System.ComponentModel;

namespace ApplicationProject.Views.DatedPageView
{
    public class DateRangeType : INotifyPropertyChanged
    {
        public enum RangeType
        {
            MONTH,
            YEAR,
            CUSTOM
        }

        public string DisplayName
        {
            get => m_Name;
            set
            {
                m_Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
            }
        }
        private string m_Name;

        public RangeType Type { get; set; }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
