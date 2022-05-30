using System;
using System.Globalization;
using System.ComponentModel;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using ApplicationProject.Views;
using ApplicationProject.Views.InterPageView;
using ApplicationProject.Views.DatedPageView;

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
                m_CurrentCulture = value ?? System.Threading.Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture ?? CultureInfo.CurrentCulture ?? CultureInfo.InvariantCulture;
                DateRangeSelectorCalendar.CurrentCulture = m_CurrentCulture;
                RefreshLocalization();
            }
        }


        #region IViewPresenter
        public IBaseView PresentedView { get; private set; }

        public bool Present(IBaseView view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));
            else if (!(view is UserControl && view.Show()))
                return false;

            if (PresentedView is ISupportOverlay overlay)
            {
                overlay.ClearOverlay();
                overlay.Overlay = null;
            }

            PresentedView = view;
            ActiveView.Content = view as UserControl;

            PresentedView?.OnCultureChanged(CurrentCulture);
            if (PresentedView is ISupportOverlay overlay2)
                overlay2.Overlay = Overlay;

            return true;
        }
        #endregion

        #region IBaseView
        public bool Show()
        {
            ShowPreview?.Invoke(this, EventArgs.Empty);

            return DateRangeTypes.Count > 0 &&
                   PageNameText?.Length > 0;
        }

        public void OnCultureChanged(CultureInfo newCulture)
        {
            CurrentCulture = newCulture;

            foreach (DateRangeType type in DateRangeTypes)
                type.OnCultureChanged(newCulture);
        }

        public void DispatchUpdate(ViewUpdate action)
        {
            Dispatcher.Invoke(() => action(this));
        }

        public event EventHandler ShowPreview;
        #endregion

        #region IDatedPageView
        public string DateRangeText => ConvertToDateRangeDisplay(DisplayedDateRange);

        public string PageNameTextKey
        {
            get => m_PageNameTextKey;
            set
            {
                m_PageNameTextKey = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageNameText)));
            }
        }
        private string m_PageNameTextKey;
        public string PageNameText => GetLocalizedString(PageNameTextKey);

        public event DateRangeTypeSelectedEventHandler DateRangeTypeSelected;
        public event DateRangeSelectedEventHandler DateRangeSelected;
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
                DateRangeSelectorCalendar.SelectionTarget = m_SelectedRangeType switch
                {
                    DateRangeType.RangeType.MONTH => RangeSelectorCalendar.RangeSelectorCalendarMode.Month,
                    DateRangeType.RangeType.YEAR => RangeSelectorCalendar.RangeSelectorCalendarMode.Year,
                    _ => throw new ArgumentOutOfRangeException(nameof(SelectedRangeType)),
                };
            }
        }
        private DateRangeType.RangeType m_SelectedRangeType;
        #endregion

        #region ISupportOverlay
        private Overlay m_Overlay;
        public Overlay Overlay
        {
            get => m_Overlay;
            set
            {
                m_Overlay = value ?? throw new ArgumentNullException(nameof(Overlay));
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
        public void RefreshLocalization()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageNameText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateRangeText)));
        }

        private string GetLocalizedString(string key)
        {
            return ApplicationProject.Resources.Locale.ResourceManager.GetString(key, CurrentCulture);
        }

        protected string ConvertToDateRangeDisplay(DateRange range)
        {
            return m_SelectedRangeType switch
            {
                DateRangeType.RangeType.MONTH => range.Start.ToString("MMMM yyyy", CurrentCulture),
                DateRangeType.RangeType.YEAR => range.Start.ToString("yyyy", CurrentCulture),
                _ => "ERROR"
            };
        }
        #endregion

        #region Handled events
        private void DateRangeSelector_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == DateRangeSelector)
            {
                DateRangeSelectorRoot.Height = DateRangeSelectorRoot.Width = DateRangeSelector.ActualWidth;
                Overlay.MoveElement(DateRangeSelectorRoot, DateRangeSelector, new Point(CalendarOffset.X, CalendarOffset.Y + DateRangeSelector.ActualHeight));
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
            m_Overlay.RemoveElement(DateRangeSelectorRoot);
        }

        private void CurrentPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Overlay.Visible)
            {
                m_Overlay.AddElement(DateRangeSelectorRoot);
                DateRangeSelectorRoot.Height = DateRangeSelectorRoot.Width = DateRangeSelector.ActualWidth;
                Overlay.MoveElement(DateRangeSelectorRoot, DateRangeSelector, new Point(CalendarOffset.X, CalendarOffset.Y + DateRangeSelector.ActualHeight));
            }
        }

        private void DateRangeTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateRangeTypeSelected?.Invoke(this, new DateRangeTypeSelectedEventArgs((DateRangeType)DateRangeTypeSelector.SelectedItem));
        }

        private void AnalysisPage_DateRangeTypesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DateRangeTypes.Count == 1)
                DateRangeTypeSelector.SelectedIndex = 0;
        }

        private void DaterRangeSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (sender == DateRangeSelectorCalendar)
            {
                foreach (RangeSelectorCalendar.DateRange range in DateRangeSelectorCalendar.SelectedRanges)
                    DateRangeSelected?.Invoke(this, new DateRangeSelectedEventArgs(new DateRange(range.Start, range.End)));
            }
        }
        #endregion
    }
}
