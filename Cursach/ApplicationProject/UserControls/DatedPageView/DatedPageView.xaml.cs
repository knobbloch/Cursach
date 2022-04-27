using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

using ApplicationProject.Views.DatedPageView;
using ApplicationProject.Views;

namespace ApplicationProject.UserControls.DatedPageView
{
    /// <summary>
    /// Interaction logic for AnalysisPageView.xaml
    /// </summary>
    public partial class DatedPageView : UserControl, IDatedPageView, ISupportOverlay, INotifyPropertyChanged, IViewPresenter
    {
        protected static readonly Point CalendarOffset = new(0, 0);

        public DatedPageView()
        {
            InitializeComponent();

            DateRangeTypes = new ObservableCollection<DateRangeType>();
            ((ObservableCollection<DateRangeType>)DateRangeTypes).CollectionChanged += AnalysisPage_DateRangeTypesChanged;
            DateRangeTypeSelector.ItemsSource = DateRangeTypes;

            DateRangeSelectorRoot = new Viewbox
            {
                Child = new Calendar()
            };

            DateRangeSelectorCalendar = (Calendar)DateRangeSelectorRoot.Child;

            DateRangeSelectorCalendar.DisplayModeChanged += DateRangeSelector_DisplayModeChanged;
        }

        public void DateRangeSelectorCalendar_Button_Click(object sender, RoutedEventArgs e)
        {
            if(e.Source is CalendarButton)
            {
                MessageBox.Show((e.Source as CalendarButton).Content.ToString());
            }
        }

        public void DateRangeSelectorCalendar_DayButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DayButtonClick!");
        }

        protected Viewbox DateRangeSelectorRoot { get; }
        protected Calendar DateRangeSelectorCalendar { get; }

        #region IBaseView
        public void Show()
        {
            Shown?.Invoke(this, EventArgs.Empty);
        }
        public void Hide()
        {
            Hidden?.Invoke(this, EventArgs.Empty);
        }
        public bool IsPresentable
        {
            get
            {
                return PageNameTextKey != null &&
                       DateRangeTypes.Count > 0;
            }
        }

        public void OnCultureChanged(System.Globalization.CultureInfo culture)
        {
            PresentedView?.OnCultureChanged(culture);
        }

        public event EventHandler Shown;
        public event EventHandler Hidden;
        #endregion

        #region IViewPresenter
        public IBaseView PresentedView { get; protected set; }

        public bool Present(IBaseView view)
        {
            if(view == null)
                throw new ArgumentNullException(nameof(view));
            else if(!view.IsPresentable || !(view is UserControl))
                return false;

            PresentedView?.Hide();
            if(PresentedView is ISupportOverlay overlay)
            {
                overlay.ClearOverlay();
                overlay.Overlay = null;
            }

            PresentedView = view;
            ActiveView.Content = view as UserControl;

            PresentedView?.Show();
            if(PresentedView is ISupportOverlay overlay2)
                overlay2.Overlay = Overlay;

            return true;
        }

        #endregion

        #region IDatedPageView
        public event EventHandler<DateRangeTypeSelectedEventArgs> DateRangeTypeSelected;
        public event EventHandler<DateRangeSelectedEventArgs> DateRangeSelected;
        public event EventHandler NextDateRangeSelected;
        public event EventHandler PreviousDateRangeSelected;

        public DateRange DateRange
        {
            get => m_DateRangeText;
            set
            {
                m_DateRangeText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateRange)));
            }
        }
        private DateRange m_DateRangeText;
        public string PageNameTextKey
        {
            get => m_PageNameText;
            set
            {
                m_PageNameText = value ?? throw new ArgumentNullException(nameof(PageNameTextKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageNameTextKey)));
            }
        }
        private string m_PageNameText;

        public ICollection<DateRangeType> DateRangeTypes { get; }

        public DateRange DateRangeBounds
        {
            set { }
        }

        public IViewPresenter PageViewPresenter { get => this; }
        #endregion

        #region ISupportOverlay
        private Overlay m_Overlay;
        public Overlay Overlay
        {
            get => m_Overlay;
            set
            {
                m_Overlay = value ?? throw new ArgumentNullException(nameof(value));
                m_Overlay.AddElement(DateRangeSelectorRoot);
                m_Overlay.BackgroundClick += Overlay_Click;
            }
        }

        public void ClearOverlay()
        {
            Overlay.RemoveElement(DateRangeSelectorRoot);
            Overlay.BackgroundClick -= Overlay_Click;
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void DateRangeSelector_Click(object sender, RoutedEventArgs e)
        {
            if(e.OriginalSource == DateRangeSelector)
            {
                DateRangeSelectorRoot.Width = DateRangeSelector.ActualWidth;
                DateRangeSelectorRoot.Height = DateRangeSelector.ActualWidth;
                //A calendar has weird blank strips above and under it each approximately 0.047 of its height, so we have to move the calendar down a bit to avoid overlaying the button
                Overlay.MoveElement(DateRangeSelectorRoot, DateRangeSelector, new Point(CalendarOffset.X, CalendarOffset.Y + DateRangeSelector.ActualHeight - 0.047*DateRangeSelector.ActualWidth));
                Overlay.Visible = DateRangeSelector.IsChecked ?? false;
                DateRangeSelectorRoot.Visibility = (DateRangeSelector.IsChecked ?? false) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void ButtonPreviousDateRange_Click(object sender, RoutedEventArgs e)
        {
            PreviousDateRangeSelected?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonNextDateRange_Click(object sender, RoutedEventArgs e)
        {
            NextDateRangeSelected?.Invoke(this, EventArgs.Empty);
        }

        private void DateRangeSelector_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {

        }

        private void Overlay_Click(object sender, EventArgs e)
        {
            Overlay.Visible = false;
            DateRangeSelector.IsChecked = false;
        }

        private void CurrentPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(Overlay.Visible)
            {
                DateRangeSelectorRoot.Width = DateRangeSelector.ActualWidth;
                DateRangeSelectorRoot.Height = DateRangeSelector.ActualWidth;
                //A calendar has weird blank strips above and under it each approximately 0.047 of its height, so we have to move the calendar down a bit to avoid overlaying the button
                Overlay.MoveElement(DateRangeSelectorRoot, DateRangeSelector, new Point(CalendarOffset.X, CalendarOffset.Y + DateRangeSelector.ActualHeight - 0.047*DateRangeSelector.ActualWidth));
            }
        }

        private void DateRangeTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateRangeTypeSelected?.Invoke(this, new DateRangeTypeSelectedEventArgs((DateRangeType)DateRangeTypeSelector.SelectedItem));
        }

        private void AnalysisPage_DateRangeTypesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(DateRangeTypes.Count == 1)
                DateRangeTypeSelector.SelectedIndex = 0;
        }
    }
}
