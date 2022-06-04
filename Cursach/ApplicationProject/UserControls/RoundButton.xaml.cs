using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;

namespace ApplicationProject.UserControls
{
    /// <summary>
    /// Interaction logic for RoundButton.xaml
    /// </summary>
    public partial class RoundButton : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public FrameworkElement ButtonContent
        {
            get => m_ButtonContent;
            set
            {
                m_ButtonContent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonContent)));
            }
        }
        private FrameworkElement m_ButtonContent;

        public FrameworkElement UnderContent
        {
            get => m_UnderContent;
            set
            {
                m_UnderContent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnderContent)));
            }
        }
        private FrameworkElement m_UnderContent;

        public Brush ButtonColor
        {
            get => m_ButtonColor;
            set
            {
                m_ButtonColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonColor)));
            }
        }
        private Brush m_ButtonColor;

        [Category("Behaviour")]
        public event RoutedEventHandler Click;

        void OnClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        public RoundButton()
        {
            InitializeComponent();
        }
    }
}