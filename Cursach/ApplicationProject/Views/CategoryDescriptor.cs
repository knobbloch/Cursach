using System;
using System.ComponentModel;

namespace ApplicationProject.Views
{
    public class CategoryDescriptor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_DisplayName;
        public string DisplayName
        {
            get => m_DisplayName;
            set
            {
                m_DisplayName = value ?? throw new ArgumentNullException(nameof(DisplayName));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
            }
        }
    }
}
