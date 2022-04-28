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
using System.Globalization;

using ApplicationProject.Views.DatedPageView;
using ApplicationProject.Views;

using ApplicationProject.UserControls;

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
            m_PageNameTextKey = "";

            InitializeComponent();

            DateRangeTypes = new ObservableCollection<DateRangeType>();
            ((ObservableCollection<DateRangeType>)DateRangeTypes).CollectionChanged += AnalysisPage_DateRangeTypesChanged;
            DateRangeTypeSelector.ItemsSource = DateRangeTypes;

            DateRangeSelectorRoot = new Viewbox
            {
                Child = new RangeSelectorCalendar()
            };

            DateRangeSelectorCalendar = (RangeSelectorCalendar)DateRangeSelectorRoot.Child;
            DateRangeSelectorCalendar.SelectionTarget = RangeSelectorCalendar.RangeSelectorCalendarMode.Month;
            DateRangeSelectorCalendar.SelectionChanged += DaterRangeSelector_SelectionChanged;
            CurrentCulture = null;
        }

        protected Viewbox DateRangeSelectorRoot { get; }
        protected RangeSelectorCalendar DateRangeSelectorCalendar { get; }
        private CultureInfo m_CurrentCulture;
        protected CultureInfo CurrentCulture
        {
            get => m_CurrentCulture;
            set
            {
                m_CurrentCulture = value ?? System.Threading.Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture;
                DateRangeSelectorCalendar.CurrentCulture = m_CurrentCulture;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageNameText)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateRangeText)));
            }
        }

        #region IBaseView
        public void Show()
        {
            Shown?.Invoke(this, EventArgs.Empty);
        }
        public void Hide()
        {
            Hidden?.Invoke(this, EventArgs.Empty);
        }
        public bool IsPresentable => DateRangeTypes.Count > 0;

        public void OnCultureChanged(CultureInfo culture)
        {
            CurrentCulture = culture;
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

        public DateRange DisplayedDateRange
        {
            get => m_DisplayedDateRange;
            set
            {
                m_DisplayedDateRange = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateRangeText)));
            }
        }
        private DateRange m_DisplayedDateRange;

        public string DateRangeText => ConvertToDateRangeDisplay(DisplayedDateRange);

        public string PageNameText => PageNameTextKey.ToString();
        public string PageNameTextKey
        {
            get => m_PageNameTextKey;
            set
            {
                m_PageNameTextKey = value ?? throw new ArgumentNullException(nameof(PageNameTextKey));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageNameText)));
            }
        }
        private string m_PageNameTextKey;

        public ICollection<DateRangeType> DateRangeTypes { get; }

        public DateRange? DateRangeBounds
        {
            get => new(DateRangeSelectorCalendar.LowerBoundary.Value, DateRangeSelectorCalendar.UpperBoundary.Value);
            set
            {
                DateRangeSelectorCalendar.LowerBoundary = value.HasValue ? value.Value.Start : null;
                DateRangeSelectorCalendar.UpperBoundary = value.HasValue ? value.Value.End : null;
            }
        }

        public DateRangeType.RangeType SelectedRangeType
        {
            get => m_SelectedRangeType;
            set
            {
                m_SelectedRangeType = value;
                switch(m_SelectedRangeType)
                {
                    case DateRangeType.RangeType.MONTH:
                        DateRangeSelectorCalendar.SelectionTarget = RangeSelectorCalendar.RangeSelectorCalendarMode.Month;
                        break;
                    case DateRangeType.RangeType.YEAR:
                        DateRangeSelectorCalendar.SelectionTarget = RangeSelectorCalendar.RangeSelectorCalendarMode.Year;
                        break;
                }
            }
        }
        private DateRangeType.RangeType m_SelectedRangeType;

        public IViewPresenter PageViewPresenter => this;
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

        #region Methods
        protected string ConvertToDateRangeDisplay(DateRange range)
        {
            switch(m_SelectedRangeType)
            {
                case DateRangeType.RangeType.MONTH:
                    return DisplayedDateRange.Start.ToString("MMMM yyyy", CurrentCulture);
                case DateRangeType.RangeType.YEAR:
                    return DisplayedDateRange.Start.ToString("yyyy", CurrentCulture);
            }
            return "ERROR";
        }
        #endregion

        #region Handled events

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

        private void DaterRangeSelector_SelectionChanged(object sender, EventArgs e)
        {
            if(sender == DateRangeSelectorCalendar)
            {
                foreach (RangeSelectorCalendar.DateRange range in DateRangeSelectorCalendar.SelectedRanges)
                    DateRangeSelected?.Invoke(this, new DateRangeSelectedEventArgs(new DateRange(range.Start, range.End)));
            }
        }

        #endregion
    }
}
