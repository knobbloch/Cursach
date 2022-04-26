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
            Single,
            SingleContinuous
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
                get => ((TextBlock)((Viewbox)Content).Child).Text;
                set => ((TextBlock)((Viewbox)Content).Child).Text = value;
            }
            public Brush TextColor
            {
                get => ((TextBlock)((Viewbox)Content).Child).Foreground;
                set => ((TextBlock)((Viewbox)Content).Child).Foreground = value ?? throw new ArgumentNullException(nameof(TextColor));
            }

            public RangeSelectorCalendarButton(RangeSelectorCalendar parent) : this(parent, "", (RangeSelectorCalendar c) => {}) {}

            public RangeSelectorCalendarButton(RangeSelectorCalendar parent, string displayName) : this(parent, displayName, (RangeSelectorCalendar c) => {}) {}

            public RangeSelectorCalendarButton(RangeSelectorCalendar parent, string displayName, Action<RangeSelectorCalendar> onClick)
            {
                ParentCalendar = parent ?? throw new ArgumentNullException(nameof(parent));
                UpdateCalendarOnClick = onClick;

                Content = new Viewbox
                {
                    Child = new TextBlock()
                };

                DisplayName = displayName;
                TextColor = Brushes.Black;
                Background = Brushes.Transparent;

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
            CurrentCulture = Thread.CurrentThread.CurrentUICulture;

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
                }

            MonthsYearsButtons = new RangeSelectorCalendarButton[MonthsYearsHeight, MonthsYearsWidth];
            for(i = 0; i < MonthsYearsHeight; i++)
                for(j = 0; j < MonthsYearsWidth; j++)
                    MonthsYearsButtons[i, j] = new RangeSelectorCalendarButton(this, (i*MonthsYearsWidth + j).ToString());

            SelectedMonthStart = DateTime.Today;
            SelectedMonthStart = SelectedMonthStart.AddDays(-SelectedMonthStart.Day + 1);
            ActiveLevel = RangeSelectorCalendarMode.Day;
            SelectionTarget = RangeSelectorCalendarMode.Day;
            SelectionMode = RangeSelectorSelectionMode.Single;

            Rebuild();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Fields
        private DateTime? m_UpperBoundary;
        private DateTime? m_LowerBoundary;
        #endregion

        #region Properties
        protected RangeSelectorCalendarButton[,] DaysButtons { get; }
        protected RangeSelectorCalendarButton[,] MonthsYearsButtons { get; }
        protected DateTime SelectedMonthStart { get; set; }

        public DateTime? UpperBoundary
        {
            get => m_UpperBoundary;
            set
            {
                m_UpperBoundary = value;
                Rebuild();
            }
        }

        public DateTime? LowerBoundary
        {
            get => m_LowerBoundary;
            set
            {
                m_LowerBoundary = value;
                Rebuild();
            }
        }

        private RangeSelectorCalendarMode m_SelectionTarget;
        public RangeSelectorCalendarMode SelectionTarget
        {
            get => m_SelectionTarget;
            set
            {
                ActiveLevel = m_SelectionTarget = value;
            }
        }

        private RangeSelectorSelectionMode m_SelectionMode;
        public RangeSelectorSelectionMode SelectionMode
        {
            get => m_SelectionMode;
            set
            {
                m_SelectionMode = value;
            }
        }

        private RangeSelectorCalendarMode m_ActiveLevel;
        protected RangeSelectorCalendarMode ActiveLevel
        {
            get => m_ActiveLevel;
            set
            {
                m_ActiveLevel = value;
                Rebuild();
            }
        }

        protected CultureInfo CurrentCulture { get; set; }
        public string DayOneAbbreviation { get => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[0].Substring(0, 1); }
        public string DayTwoAbbreviation { get => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[1].Substring(0, 1); }
        public string DayThreeAbbreviation { get => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[2].Substring(0, 1); }
        public string DayFourAbbreviation { get => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[3].Substring(0, 1); }
        public string DayFiveAbbreviation { get => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[4].Substring(0, 1); }
        public string DaySixAbbreviation { get => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[5].Substring(0, 1); }
        public string DaySevenAbbreviation { get => CurrentCulture.DateTimeFormat.AbbreviatedDayNames[6].Substring(0, 1); }

        #endregion

        #region Methods
        public void ResetSelection()
        {

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

            if(ActiveLevel == RangeSelectorCalendarMode.Day)
            {
                DaysGrid.Visibility = Visibility.Visible;
                MonthsYearsGrid.Visibility = Visibility.Hidden;

                DateTime day = SelectedMonthStart;
                while(day.DayOfWeek != CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                    day = day.AddDays(-1);

                int i, j;
                for(i = 0; i < DaysHeight; i++)
                    for(j = 0; j < DaysWidth; j++)
                    {
                        if((!LowerBoundary.HasValue || day >= LowerBoundary.Value) && (!UpperBoundary.HasValue || day <= UpperBoundary.Value))
                        {
                            DateTime localDay = day;
                            DaysButtons[i, j].Visibility = Visibility.Visible;
                            DaysButtons[i, j].DisplayName = day.Day.ToString();
                            DaysButtons[i, j].UpdateCalendarOnClick = (_) => MessageBox.Show(localDay.ToString(CurrentCulture.DateTimeFormat));
                            DaysButtons[i, j].TextColor = day.Month == SelectedMonthStart.Month ? Brushes.Black : Brushes.Gray;
                        }
                        else
                            DaysButtons[i, j].Visibility = Visibility.Hidden;

                        day = day.AddDays(1);
                    }
            }

            
        }
        #endregion

        #region Events
        private void PreviousGroup_Click(object sender, RoutedEventArgs e)
        {

        }
        protected void Self_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender == this && !Thread.CurrentThread.CurrentUICulture.Equals(CurrentCulture))
            {
                CurrentCulture = Thread.CurrentThread.CurrentUICulture;
                Rebuild();
            }
        }
        #endregion
    }
}
