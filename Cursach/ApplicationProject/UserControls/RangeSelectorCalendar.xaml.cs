using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace ApplicationProject.UserControls
{
    /// <summary>
    /// Interaction logic for RangeSelectorCalendar.xaml
    /// </summary>
    public partial class RangeSelectorCalendar : UserControl, INotifyPropertyChanged
    {
        #region Nested Types
        public enum RangeSelectorCalendarMode
        {
            Day,
            Month,
            Year
        }

        public enum RangeSelectorSelectionMode
        {
            Single
        }

        public struct DateRange : IEquatable<DateRange>
        {
            public DateTime Start { get; }
            public DateTime End { get; }

            public DateRange(DateTime start, DateTime end)
            {
                if(start > end)
                    throw new ArgumentException("A DateRange's start should preceed its end.");

                Start = start;
                End = end;
            }

            public static bool operator == (DateRange left, DateRange right) => left.Equals(right);
            public static bool operator != (DateRange left, DateRange right) => !left.Equals(right);

            public bool Equals(DateRange other)
            {
                return other.Start.Equals(Start) && other.End.Equals(End);
            }

            public override bool Equals(object obj)
            {
                return obj is DateRange other && other.Start.Equals(Start) && other.End.Equals(End);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Start, End);
            }
        }

        protected class RangeSelectorCalendarButton : Button
        {
            private Action<RangeSelectorCalendar> m_UpdateCalendarOnClick;

            public RangeSelectorCalendar ParentCalendar { get; }
            public Action<RangeSelectorCalendar> UpdateCalendarOnClick
            {
                get => m_UpdateCalendarOnClick;
                set => m_UpdateCalendarOnClick = value ?? throw new ArgumentNullException(nameof(UpdateCalendarOnClick));
            }
            public string DisplayName
            {
                get => ((TextBlock)Content).Text;
                set => ((TextBlock)Content).Text = value;
            }
            public Brush TextColor
            {
                get => ((TextBlock)Content).Foreground;
                set => ((TextBlock)Content).Foreground = value ?? throw new ArgumentNullException(nameof(TextColor));
            }

            public RangeSelectorCalendarButton(RangeSelectorCalendar parent) : this(parent, "", (RangeSelectorCalendar c) => {}) {}

            public RangeSelectorCalendarButton(RangeSelectorCalendar parent, string displayName) : this(parent, displayName, (RangeSelectorCalendar c) => {}) {}

            public RangeSelectorCalendarButton(RangeSelectorCalendar parent, string displayName, Action<RangeSelectorCalendar> onClick)
            {
                ParentCalendar = parent ?? throw new ArgumentNullException(nameof(parent));
                UpdateCalendarOnClick = onClick;

                Content = new TextBlock();

                DisplayName = displayName;
                TextColor = Brushes.Black;
                Background = Brushes.Transparent;
                BorderThickness = new Thickness(0);

                Click += Self_Click;
            }

            private void Self_Click(object sender, RoutedEventArgs e)
            {
                UpdateCalendarOnClick.Invoke(ParentCalendar);
            }
        }
        #endregion

        protected const int DaysWidth = 7,
                            DaysHeight = 6,
                            MonthsYearsWidth = 4,
                            MonthsYearsHeight = 3;

        public RangeSelectorCalendar()
        {
            m_CurrentCulture = Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture;
            LowerBoundaryInternal = CurrentCulture.Calendar.MinSupportedDateTime;
            UpperBoundaryInternal = CurrentCulture.Calendar.MaxSupportedDateTime;

            InitializeComponent();

            DaysButtons = new RangeSelectorCalendarButton[DaysHeight, DaysWidth];
            int i, j;
            for(i = 0; i < DaysHeight; i++)
                for(j = 0; j < DaysWidth; j++)
                {
                    DaysButtons[i, j] = new RangeSelectorCalendarButton(this, (i*DaysWidth + j).ToString());
                    _ = DaysGrid.Children.Add(DaysButtons[i, j]);
                    DaysButtons[i, j].SetValue(Grid.RowProperty, (i + 1) * 2);
                    DaysButtons[i, j].SetValue(Grid.ColumnProperty, j * 2);
                    DaysButtons[i, j].HorizontalAlignment = HorizontalAlignment.Stretch;
                    DaysButtons[i, j].VerticalAlignment = VerticalAlignment.Stretch;
                }

            MonthsYearsButtons = new RangeSelectorCalendarButton[MonthsYearsHeight, MonthsYearsWidth];
            for(i = 0; i < MonthsYearsHeight; i++)
                for(j = 0; j < MonthsYearsWidth; j++)
                {
                    MonthsYearsButtons[i, j] = new RangeSelectorCalendarButton(this, (i*MonthsYearsWidth + j).ToString());
                    _ = MonthsYearsGrid.Children.Add(MonthsYearsButtons[i, j]);
                    MonthsYearsButtons[i, j].SetValue(Grid.RowProperty, i * 2);
                    MonthsYearsButtons[i, j].SetValue(Grid.ColumnProperty, j * 2);
                    MonthsYearsButtons[i, j].HorizontalAlignment = HorizontalAlignment.Stretch;
                    MonthsYearsButtons[i, j].VerticalAlignment = VerticalAlignment.Stretch;
                }

            SelectedPeriodStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            m_SelectionTarget = RangeSelectorCalendarMode.Day;
            m_SelectionMode = RangeSelectorSelectionMode.Single;
            m_ActiveLevel = RangeSelectorCalendarMode.Day;

            SelectedRangesInternal = new List<DateRange>();

            Rebuild();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        
        #region Properties
        protected List<DateRange> SelectedRangesInternal { get; }
        public IReadOnlyCollection<DateRange> SelectedRanges
        {
            get => SelectedRangesInternal;
        }

        protected RangeSelectorCalendarButton[,] DaysButtons { get; }
        protected RangeSelectorCalendarButton[,] MonthsYearsButtons { get; }

        /// <summary>
        /// Stores the first day of the currently selected period (based on ActiveLevel)
        /// </summary>
        /// <remarks>
        /// Warning: changing the value does not cause the calendar to be rebuilt to match the new data. Call Rebuild() instead.
        /// </remarks>
        protected DateTime SelectedPeriodStart { get; set; }

        private DateTime m_UpperBoundary;
        /// <summary>
        /// Non-nullable version of UpperBoundary for internal use
        /// </summary>
        /// <remarks>
        /// Cannot be set to a value greater than CurrentCulture.Calendar.MaxSupportedDateTime
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException" />
        protected DateTime UpperBoundaryInternal
        {
            get => m_UpperBoundary;
            set
            {
                if(value > CurrentCulture.Calendar.MaxSupportedDateTime)
                    throw new ArgumentOutOfRangeException(nameof(UpperBoundaryInternal));

                m_UpperBoundary = value;
            }
        }
        /// <summary>
        /// Stores the upper boundary of all the dates from which selection is available.
        /// Can be set to null to disable.
        /// Changing it causes the calendar to instantly update itself to match the change.
        /// </summary>
        /// <remarks>
        /// Cannot be set to a value greater than CurrentCulture.Calendar.MaxSupportedDateTime
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException" />
        public DateTime? UpperBoundary
        {
            get => UpperBoundaryInternal;
            set
            {
                UpperBoundaryInternal = value ?? CurrentCulture.Calendar.MaxSupportedDateTime;
                Rebuild();
            }
        }

        private DateTime m_LowerBoundary;
        /// <summary>
        /// Non-nullable version of LowerBoundary for internal use
        /// </summary>
        /// <remarks>
        /// Cannot be set to a value less than CurrentCulture.Calendar.MinSupportedDateTime
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException" />
        protected DateTime LowerBoundaryInternal
        {
            get => m_LowerBoundary;
            set
            {
                if(value < CurrentCulture.Calendar.MinSupportedDateTime)
                    throw new ArgumentOutOfRangeException(nameof(LowerBoundaryInternal));

                m_LowerBoundary = value;
            }
        }
        /// <summary>
        /// Stores the lower boundary of all the dates from which selection is available.
        /// Can be set to null to disable.
        /// Changing it causes the calendar to instantly update itself to match the change.
        /// </summary>
        /// <remarks>
        /// Cannot be set to a value less than CurrentCulture.Calendar.MinSupportedDateTime
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException" />
        public DateTime? LowerBoundary
        {
            get => LowerBoundaryInternal;
            set
            {
                LowerBoundaryInternal = value ?? CurrentCulture.Calendar.MinSupportedDateTime;
                Rebuild();
            }
        }

        private RangeSelectorCalendarMode m_SelectionTarget;
        /// <summary>
        /// The shortest selectable period for this calendar.
        /// </summary>
        public RangeSelectorCalendarMode SelectionTarget
        {
            get => m_SelectionTarget;
            set
            {
                ActiveLevel = m_SelectionTarget = value;
            }
        }

        private RangeSelectorSelectionMode m_SelectionMode;
        /// <summary>
        /// The selection mode.
        /// </summary>
        public RangeSelectorSelectionMode SelectionMode
        {
            get => m_SelectionMode;
            set
            {
                m_SelectionMode = value;
            }
        }

        private RangeSelectorCalendarMode m_ActiveLevel;
        /// <summary>
        /// Currently displayed level (days, months, years).
        /// </summary>
        /// <remarks>
        /// Changing it causes the calendar to be rebuilt to match the change.
        /// </remarks>
        protected RangeSelectorCalendarMode ActiveLevel
        {
            get => m_ActiveLevel;
            set
            {
                if(value < SelectionTarget)
                    throw new ArgumentOutOfRangeException(nameof(ActiveLevel), "Active level cannot be lower than SelectionTarget");

                m_ActiveLevel = value;
                Rebuild();
            }
        }

        private CultureInfo m_CurrentCulture;
        /// <summary>
        /// Sets the culture used to display this calendar.
        /// </summary>
        /// <remarks>
        /// If set to null, reverts to the current thread's UI culture.
        /// </remarks>
        public CultureInfo CurrentCulture
        {
            get => m_CurrentCulture;
            set
            {
                m_CurrentCulture = value ?? Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture;
                Rebuild();
            }
        }
        public string DayOneAbbreviation => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[0].Substring(0, 1); 
        public string DayTwoAbbreviation => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[1].Substring(0, 1);
        public string DayThreeAbbreviation => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[2].Substring(0, 1);
        public string DayFourAbbreviation => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[3].Substring(0, 1);
        public string DayFiveAbbreviation => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[4].Substring(0, 1);
        public string DaySixAbbreviation => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[5].Substring(0, 1);
        public string DaySevenAbbreviation => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[6].Substring(0, 1);
        public string LevelHeader
        {
            get
            {
                if(ActiveLevel == RangeSelectorCalendarMode.Day)
                    return SelectedPeriodStart.ToString(CurrentCulture.DateTimeFormat.YearMonthPattern, CurrentCulture);
                else if(ActiveLevel == RangeSelectorCalendarMode.Month)
                    return SelectedPeriodStart.ToString("yyyy", CurrentCulture);
                else if(ActiveLevel == RangeSelectorCalendarMode.Year)
                    return SelectedPeriodStart.ToString("yyyy", CurrentCulture) + "-" + SelectedPeriodStart.AddYears(11).ToString("yyyy", CurrentCulture);
                else
                    throw new ArgumentOutOfRangeException(nameof(ActiveLevel), "Invalid active level set");
            }
        }

        #endregion

        #region Methods
        protected void OnDaySelected(DateTime day)
        {
            if(day.Month == SelectedPeriodStart.Month)
                AddToSelection(day, day);
            else
            {
                SelectedPeriodStart = SelectedPeriodStart.AddMonths(day < SelectedPeriodStart ? -1 : 1 );
                Rebuild();
            }
        }

        protected void OnMonthSelected(DateTime monthStart)
        {
            if(SelectionTarget == RangeSelectorCalendarMode.Month)
                AddToSelection(monthStart, new DateTime(monthStart.Year, monthStart.Month, DateTime.DaysInMonth(monthStart.Year, monthStart.Month)));
            else
            {
                SelectedPeriodStart = monthStart;
                ActiveLevel = RangeSelectorCalendarMode.Day;
            }
        }

        protected void OnYearSelected(DateTime yearStart)
        {
            if(SelectionTarget == RangeSelectorCalendarMode.Year)
                AddToSelection(yearStart, new DateTime(yearStart.Year, 12, DateTime.DaysInMonth(yearStart.Year, 12)));
            else
            {
                SelectedPeriodStart = yearStart;
                ActiveLevel = RangeSelectorCalendarMode.Month;
            }
        }

        protected void AddToSelection(DateTime start, DateTime end)
        {
            if(SelectionMode == RangeSelectorSelectionMode.Single)
            {
                SelectedRangesInternal.Clear();
                SelectedRangesInternal.Add(new DateRange(start, end));
            }

            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Rebuild()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DayOneAbbreviation)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DayTwoAbbreviation)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DayThreeAbbreviation)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DayFourAbbreviation)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DayFiveAbbreviation)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DaySixAbbreviation)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DaySevenAbbreviation)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LevelHeader)));

            if(ActiveLevel == RangeSelectorCalendarMode.Day)
            {
                DaysGrid.Visibility = Visibility.Visible;
                MonthsYearsGrid.Visibility = Visibility.Hidden;

                //Account for possibiliy of exceeding supported datetime
                DateTime day = SelectedPeriodStart.AddDays(-1);
                while(day.DayOfWeek != CurrentCulture.DateTimeFormat.FirstDayOfWeek && day > LowerBoundaryInternal)
                    day = day.AddDays(-1);

                int i, j;
                for(i = 0; i < DaysHeight; i++)
                    for(j = 0; j < DaysWidth; j++)
                    {
                        if(day >= LowerBoundaryInternal && day <= UpperBoundaryInternal)
                        {
                            DateTime localDay = day;
                            DaysButtons[i, j].Visibility = Visibility.Visible;
                            DaysButtons[i, j].DisplayName = day.ToString("%d", CurrentCulture);
                            DaysButtons[i, j].UpdateCalendarOnClick = (_) => OnDaySelected(localDay);
                            DaysButtons[i, j].TextColor = day.Month == SelectedPeriodStart.Month ? Brushes.Black : Brushes.Gray;
                            day = day.AddDays(1);
                        }
                        else
                            DaysButtons[i, j].Visibility = Visibility.Hidden;
                    }
            }
            else if(ActiveLevel == RangeSelectorCalendarMode.Month)
            {
                DaysGrid.Visibility = Visibility.Hidden;
                MonthsYearsGrid.Visibility = Visibility.Visible;

                DateTime month = SelectedPeriodStart;

                //Offset lower boundary to be the next sequential day of the previous month
                //This will guarantee that if month.Month == lowerboundary.Month, it will be included, but not if month.Month == lowerboundary.Month - 1
                DateTime lowerBoundary;
                try
                {
                    lowerBoundary = LowerBoundaryInternal.AddMonths(-1).AddDays(1);
                }
                catch
                {
                    lowerBoundary = LowerBoundaryInternal;
                }
                

                int i, j;
                for(i = 0; i < MonthsYearsHeight; i++)
                    for(j = 0; j < MonthsYearsWidth; j++)
                    {
                        if(month >= lowerBoundary && month <= UpperBoundaryInternal)
                        {
                            DateTime localMonth = month;
                            MonthsYearsButtons[i, j].Visibility = Visibility.Visible;
                            MonthsYearsButtons[i, j].DisplayName = localMonth.ToString("MMM", CurrentCulture);
                            MonthsYearsButtons[i, j].UpdateCalendarOnClick = (_) => OnMonthSelected(localMonth);
                            month = month.AddMonths(1);
                        }
                        else
                            MonthsYearsButtons[i, j].Visibility = Visibility.Hidden;
                    }
            }
            else if(ActiveLevel == RangeSelectorCalendarMode.Year)
            {
                DaysGrid.Visibility = Visibility.Hidden;
                MonthsYearsGrid.Visibility = Visibility.Visible;

                DateTime year = SelectedPeriodStart;

                //Offset lower boundary to be the next sequential day of the previous month
                //This will guarantee that if month.Month == lowerboundary.Month, it will be included, but not if month.Month == lowerboundary.Month - 1

                DateTime lowerBoundary;
                try
                {
                    lowerBoundary = LowerBoundaryInternal.AddYears(-1).AddDays(1);
                }
                catch
                {
                    lowerBoundary = LowerBoundaryInternal;
                }

                int i, j;
                for(i = 0; i < MonthsYearsHeight; i++)
                    for(j = 0; j < MonthsYearsWidth; j++)
                    {
                        if(year >= lowerBoundary && year <= UpperBoundaryInternal)
                        {
                            DateTime localYear = year;
                            MonthsYearsButtons[i, j].Visibility = Visibility.Visible;
                            MonthsYearsButtons[i, j].DisplayName = localYear.ToString("yyyy", CurrentCulture);
                            MonthsYearsButtons[i, j].UpdateCalendarOnClick = (_) => OnYearSelected(localYear);
                            year = year.AddYears(1);
                        }
                        else
                            MonthsYearsButtons[i, j].Visibility = Visibility.Hidden;
                    }
            }
            
        }
        #endregion

        #region Events
        public event EventHandler SelectionChanged;
        #endregion

        #region Handled Events
        private void PreviousGroup_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(ActiveLevel == RangeSelectorCalendarMode.Day)
                SelectedPeriodStart = SelectedPeriodStart.AddMonths(-1);
            else if(ActiveLevel == RangeSelectorCalendarMode.Month)
                SelectedPeriodStart = SelectedPeriodStart.AddYears(-1);
            else if(ActiveLevel == RangeSelectorCalendarMode.Year)
                SelectedPeriodStart = SelectedPeriodStart.AddYears(-10);

            Rebuild();
        }

        private void NextGroup_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(ActiveLevel == RangeSelectorCalendarMode.Day)
                SelectedPeriodStart = SelectedPeriodStart.AddMonths(1);
            else if(ActiveLevel == RangeSelectorCalendarMode.Month)
                SelectedPeriodStart = SelectedPeriodStart.AddYears(1);
            else if(ActiveLevel == RangeSelectorCalendarMode.Year)
                SelectedPeriodStart = SelectedPeriodStart.AddYears(10);

            Rebuild();
        }

        private void HigherLevel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(ActiveLevel == RangeSelectorCalendarMode.Day)
            {
                //Set it to the beginning of this year
                SelectedPeriodStart = new DateTime(SelectedPeriodStart.Year, 1, 1);
                ActiveLevel = RangeSelectorCalendarMode.Month;
            }
            else if(ActiveLevel == RangeSelectorCalendarMode.Month)
            {
                //Set it to the year before the beginning of this decade
                SelectedPeriodStart = SelectedPeriodStart.AddYears(-((SelectedPeriodStart.Year % 10) + 1));
                ActiveLevel = RangeSelectorCalendarMode.Year;
            }
        }
        #endregion
    }
}
