using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.ComponentModel;
using System.Reflection;

namespace ApplicationProject.UserControls
{
    /// <summary>
    /// Interaction logic for BarChart.xaml
    /// </summary>
    public sealed partial class BarChart : UserControl
    {
        #region Nested Types
        private enum BarsUpdateMode
        {
            Positive,
            Negative,
            All
        }

        private sealed class Bar
        {
            public Bar(BarChart parent, object item, string valueBinding = "value")
            {
                Parent = parent;
                m_ValueBinding = valueBinding;
                m_Item = item;

                Type type = Item.GetType();
                MemberInfo[] members = type.GetMember(valueBinding, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);

                if (members.Length != 1)
                    throw new ArgumentException($"Unable to bind to {valueBinding} on {type.Name} - can't find matching public property", nameof(valueBinding));

                PropertyInfo info = (PropertyInfo)members[0];

                if (!info.PropertyType.IsPrimitive)
                    throw new ArgumentException($"Unable to bind to {valueBinding} on {type.Name} - not a primitive type", nameof(valueBinding));

                BindingTarget = info;

                m_Value = Convert.ToDouble(BindingTarget.GetValue(Item));
            }

            private PropertyInfo BindingTarget { get; set; }

            public BarChart Parent { get; }
            public bool IsPositive { get; set; }

            private double m_Value;
            public double Value
            {
                get => (Item is INotifyPropertyChanged) ? m_Value : Convert.ToDouble(BindingTarget.GetValue(Item));
                private set
                {
                    m_Value = value;
                    Parent.UpdateBar(Parent.ItemToBarMapping[Item]);
                }
            }

            private string m_ValueBinding;
            public string ValueSource
            {
                get => m_ValueBinding;
                set
                {
                    if (value == null)
                        throw new ArgumentNullException(nameof(ValueSource));

                    Type type = Item.GetType();
                    MemberInfo[] members = type.GetMember(value, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);

                    if (members.Length != 1)
                        throw new ArgumentException($"Unable to bind to {value} on {type.Name} - can't find matching public property", nameof(ValueSource));

                    PropertyInfo info = (PropertyInfo)members[0];

                    if (!info.PropertyType.IsPrimitive)
                        throw new ArgumentException($"Unable to bind to {value} on {type.Name} - not a primitive type", nameof(ValueSource));

                    BindingTarget = info;
                    m_ValueBinding = value;
                    Parent.UpdateBar(Parent.ItemToBarMapping[Item]);
                }
            }

            private object m_Item;
            public object Item
            {
                get => m_Item;
                set
                {
                    if (value == null)
                        throw new ArgumentNullException(nameof(Item));

                    Type type = value.GetType();
                    MemberInfo[] members = type.GetMember(ValueSource, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);

                    if (members.Length != 1)
                        throw new ArgumentException($"Unable to bind to {value} on {type.Name} - can't find matching public property", nameof(Item));

                    PropertyInfo info = (PropertyInfo)members[0];

                    if (!info.PropertyType.IsPrimitive)
                        throw new ArgumentException($"Unable to bind to {value} on {type.Name} - not a primitive type", nameof(Item));

                    BindingTarget = info;
                    m_Item = value;
                    Parent.UpdateBar(Parent.ItemToBarMapping[Item]);
                }
            }

            private FrameworkElement m_BarDisplay;
            public FrameworkElement BarDisplay
            {
                get => m_BarDisplay;
                set
                {
                    BarRectangle = value.FindName("BarRectangle") as Rectangle ?? throw new ArgumentException("The provided root control doesn't have a rectangle child named BarRectangle", nameof(BarDisplay));
                    BarText = value.FindName("BarText") as TextBlock ?? throw new ArgumentException("The provided root control doesn't have a textblock child named BarText", nameof(BarDisplay));
                    m_BarDisplay = value;
                }
            }
            public TextBlock BarText { get; private set; }
            public Rectangle BarRectangle { get; private set; }
            public FrameworkElement BarTitle { get; set; }

            private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (sender == Item && BindingTarget.Name == e.PropertyName)
                    Value = Convert.ToDouble(BindingTarget.GetValue(Item));
            }
        }
        #endregion

        public BarChart() : this(null) { }

        public BarChart(Func<double, double> valueProcessor = null)
        {
            m_ValueProcessor = valueProcessor ?? ((double v) => v);
            m_BarWidth = 50.0;
            ItemToBarMapping = new();
            Bars = new();
            m_MaxValue = 0;
            m_MinValue = 0;
            BarValueFormat = null;

            InitializeComponent();

            PositiveBarTemplate = (DataTemplate)FindResource("PositiveBarTemplate");
            NegativeBarTemplate = (DataTemplate)FindResource("NegativeBarTemplate");
            BarTitleTemplate = (DataTemplate)FindResource("DefaultTitleTemplate");
        }

        #region Control Properties
        /// <summary>
        /// Current width of a single bar
        /// </summary>
        public double BarWidth
        {
            get => m_BarWidth;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(BarWidth));

                m_BarWidth = value;

                foreach(Bar b in Bars)
                    b.BarRectangle.Width = value;
            }
        }
        private double m_BarWidth;
        /// <summary>
        /// Current number of bars
        /// </summary>
        public int BarsCount => BarsGrid.ColumnDefinitions.Count;
        /// <summary>
        /// A collection that is used to generate bars of this chart
        /// </summary>
        public IEnumerable BarsSource
        {
            get => m_BarsSource;
            set
            {
                if (m_BarsSource is INotifyCollectionChanged col)
                    col.CollectionChanged -= Self_OnCollectionChanged;

                m_BarsSource = value;

                if (m_BarsSource is INotifyCollectionChanged col2)
                    col2.CollectionChanged += Self_OnCollectionChanged;

                Rebuild();
            }
        }
        private IEnumerable m_BarsSource;
        /// <summary>
        /// A function applied to the values before they are used to determine the height of the bars.
        /// Can be used to distort the bars.
        /// If set to null, the values remain unchanged.
        /// </summary>
        public Func<double, double> ValueProcessor
        {
            get => m_ValueProcessor;
            set
            {
                m_ValueProcessor = value ?? ((double v) => v);
                UpdateBars();
            }
        }
        private Func<double, double> m_ValueProcessor;

        /// <summary>
        /// A style applied to a positive bar's rectangle
        /// </summary>
        public Style PositiveBarStyle { get; set; }
        /// <summary>
        /// A style applied to a negative bar's rectangle
        /// </summary>
        public Style NegativeBarStyle { get; set; }

        public DataTemplate BarTitleTemplate { get; set; }

        /// <summary>
        /// The name of the property on the objects used to determine the value of the bars
        /// </summary>
        public string ValueSource
        {
            get => m_ValueSource;
            set
            {
                m_ValueSource = value;
                foreach (Bar bar in Bars)
                    bar.ValueSource = m_ValueSource;
            }
        }
        private string m_ValueSource;

        public string BarValueFormat
        {
            get => m_BarValueFormat;
            set => m_BarValueFormat = value ?? "{0}";
        }
        private string m_BarValueFormat;
        #endregion

        #region Internal properties
        private Dictionary<object, int> ItemToBarMapping { get; }
        private List<Bar> Bars { get; }
        private double m_MaxValue;
        private double MaxValue
        {
            get => m_MaxValue;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(MaxValue));

                m_MaxValue = value;
                UpdateBars(BarsUpdateMode.Positive);
            }
        }
        private double m_MinValue;
        private double MinValue
        {
            get => m_MinValue;
            set
            {
                if (value >= 0)
                    throw new ArgumentOutOfRangeException(nameof(MinValue));

                m_MinValue = value;
                UpdateBars(BarsUpdateMode.Negative);
            }
        }
        private DataTemplate PositiveBarTemplate { get; }
        private DataTemplate NegativeBarTemplate { get; }

        private const int PositiveBarRow = 0;
        private const int NegativeBarRow = 2;
        private const int TitleBarRow = 3;
        #endregion

        #region Methods
        /// <summary>
        /// Adds a bar and associates an item with it
        /// </summary>
        /// <param name="item">An item to add a bar for</param>
        private void AddBar(object item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            BarsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)});
            HalvesSeparator.SetValue(Grid.ColumnSpanProperty, BarsGrid.ColumnDefinitions.Count);

            Bar bar = new(this, item, ValueSource);
            double value = bar.Value;
            double processedValue = ValueProcessor(value);

            if (processedValue > MaxValue)
                MaxValue = processedValue;
            else if(processedValue < MinValue)
                MinValue = processedValue;

            Bars.Add(bar);
            int index = Bars.Count - 1;

            ItemToBarMapping.Add(item, index);

            bar.IsPositive = processedValue >= 0;

            _ = BarsGrid.Children.Add(bar.BarDisplay = (FrameworkElement)(bar.IsPositive ? PositiveBarTemplate.LoadContent() : NegativeBarTemplate.LoadContent()));
            bar.BarDisplay.Style = bar.IsPositive ? PositiveBarStyle : NegativeBarStyle;
            bar.BarRectangle.Height = Math.Abs(processedValue / Math.Max(MaxValue, MinValue) * (BarsGrid.RowDefinitions[bar.IsPositive ? PositiveBarRow : NegativeBarRow].ActualHeight - bar.BarText.ActualHeight));
            bar.BarRectangle.Width = BarWidth;
            bar.BarText.Text = string.Format(System.Threading.Thread.CurrentThread.CurrentUICulture, BarValueFormat, value);
            bar.BarDisplay.SetValue(Grid.RowProperty, bar.IsPositive ? PositiveBarRow : NegativeBarRow);
            bar.BarDisplay.SetValue(Grid.ColumnProperty, index);

            _ = BarsGrid.Children.Add(bar.BarTitle = (FrameworkElement)BarTitleTemplate.LoadContent());
            bar.BarTitle.DataContext = item;
            bar.BarTitle.UpdateLayout();
            bar.BarTitle.SetValue(Grid.RowProperty, TitleBarRow);
            bar.BarTitle.SetValue(Grid.ColumnProperty, index);

            UpdateLayout();
        }

        /// <summary>
        /// Remove a bar associated with an item
        /// </summary>
        /// <param name="item">An item with which the bar is associated</param>
        private void RemoveBar(object item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            int index;

            if (!ItemToBarMapping.TryGetValue(item, out index))
                throw new ArgumentException("The item has no matching bar", nameof(item));


            BarsGrid.ColumnDefinitions.RemoveAt(BarsGrid.ColumnDefinitions.Count - 1);
            HalvesSeparator.SetValue(Grid.ColumnSpanProperty, BarsGrid.ColumnDefinitions.Count);

            Bar bar = Bars[index];
            Bars.RemoveAt(index);
            ItemToBarMapping.Remove(bar.Item);

            BarsGrid.Children.Remove(bar.BarDisplay);
            BarsGrid.Children.Remove(bar.BarTitle);

            for (int i = index; i < Bars.Count; i++)
            {
                bar = Bars[index];
                bar.BarDisplay.SetValue(Grid.RowProperty, i);
                bar.BarTitle.SetValue(Grid.RowProperty, i);
                _ = ItemToBarMapping.Remove(bar.Item);
                ItemToBarMapping.Add(bar.Item, i);
            }


            if (bar.Value == MaxValue)
            {
                double max = 0;
                double value;
                foreach (Bar b in Bars)
                {
                    value = ValueProcessor(b.Value);
                    if (value > max)
                        max = value;
                }
                MaxValue = max;
            }
            else if (bar.Value == MinValue)
            {
                double min = 0;
                double value;
                foreach (Bar b in Bars)
                {
                    value = ValueProcessor(b.Value);
                    if (value < min)
                        min = value;
                }
                MinValue = min;
            }

            UpdateLayout();
        }

        /// <summary>
        /// Replaces an item associated with a bar with another item without removing the bar
        /// </summary>
        /// <param name="oldItem">An old item</param>
        /// <param name="newItem">A new item to replace the old one with</param>
        private void ReplaceBarItem(object oldItem, object newItem)
        {
            if (oldItem == null)
                throw new ArgumentNullException(nameof(oldItem));
            else if (newItem == null)
                throw new ArgumentNullException(nameof(newItem));

            int index;

            if (!ItemToBarMapping.TryGetValue(oldItem, out index))
                throw new ArgumentException("The old item has no matching bar", nameof(oldItem));

            Bar bar = Bars[index];
            bar.Item = newItem;
            UpdateBar(index, bar, ValueProcessor(bar.Value));
        }

        /// <summary>
        /// Updates a bar's graphics
        /// </summary>
        /// <param name="barIndex">The index of a bar to update</param>
        public void UpdateBar(int barIndex)
        {
            if(barIndex < 0 || barIndex >= Bars.Count)
                throw new ArgumentOutOfRangeException(nameof(barIndex));

            Bar bar = Bars[barIndex];
            UpdateBar(barIndex, bar, ValueProcessor(bar.Value));
        }

        private void UpdateBar(int barIndex, Bar bar, double value)
        {
            if (value > MaxValue)
                MaxValue = value;
            else if (value < MinValue)
                MinValue = value;
            else
            {
                if (value >= 0 && !bar.IsPositive)
                {
                    BarsGrid.Children.Remove(bar.BarDisplay);
                    _ = BarsGrid.Children.Add(bar.BarDisplay = (FrameworkElement)PositiveBarTemplate.LoadContent());
                    bar.BarDisplay.SetValue(Grid.RowProperty, PositiveBarRow);
                    bar.BarDisplay.SetValue(Grid.ColumnProperty, barIndex);
                    bar.BarDisplay.Style = PositiveBarStyle;
                    bar.IsPositive = true;
                }
                else if (value < 0 && bar.IsPositive)
                {
                    BarsGrid.Children.Remove(bar.BarDisplay);
                    _ = BarsGrid.Children.Add(bar.BarDisplay = (FrameworkElement)NegativeBarTemplate.LoadContent());
                    bar.BarDisplay.SetValue(Grid.RowProperty, NegativeBarRow);
                    bar.BarDisplay.SetValue(Grid.ColumnProperty, barIndex);
                    bar.BarDisplay.Style = NegativeBarStyle;
                    bar.IsPositive = false;
                }
                bar.BarRectangle.Height = Math.Abs(value / Math.Max(MaxValue, MinValue) * (BarsGrid.RowDefinitions[bar.IsPositive ? PositiveBarRow : NegativeBarRow].ActualHeight - bar.BarText.ActualHeight));
                bar.BarText.Text = string.Format(System.Threading.Thread.CurrentThread.CurrentUICulture, BarValueFormat, value);
                bar.BarTitle.UpdateLayout();
            }
        }

        /// <summary>
        /// Fully rebuilds this bar chart
        /// </summary>
        private void Rebuild()
        {
            foreach (Bar b in Bars)
            {
                BarsGrid.Children.Remove(b.BarDisplay);
                BarsGrid.Children.Remove(b.BarTitle);
            }

            Bars.Clear();

            foreach (object item in BarsSource)
                AddBar(item);

            UpdateLayout();
        }

        /// <summary>
        /// Updates the height of the bars based on their values
        /// </summary>
        private void UpdateBars(BarsUpdateMode mode = BarsUpdateMode.All)
        {
            double value;
            Bar bar;
            if (mode == BarsUpdateMode.Positive)
            {
                for (int i = 0; i < Bars.Count; i++)
                {
                    bar = Bars[i];
                    value = ValueProcessor(bar.Value);
                    if (value >= 0)
                        UpdateBar(i, bar, value);
                }
            }
            else if (mode == BarsUpdateMode.Negative)
            {
                for (int i = 0; i < Bars.Count; i++)
                {
                    bar = Bars[i];
                    value = ValueProcessor(bar.Value);
                    if (value < 0)
                        UpdateBar(i, bar, value);
                }
            }
            else
            {
                for (int i = 0; i < Bars.Count; i++)
                    UpdateBar(i);
            }
        }
        #endregion

        #region Handled events
        private void Self_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender == BarsSource)
            {
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
                    AddBar(e.NewItems[0]);
                else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
                    RemoveBar(e.OldItems[0]);
                else if (e.Action == NotifyCollectionChangedAction.Replace && e.OldItems != null && e.NewItems != null)
                    ReplaceBarItem(e.OldItems[0], e.NewItems[0]);
                else if (e.Action == NotifyCollectionChangedAction.Reset)
                    Rebuild();
            }
        }
        #endregion

        private void BarsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBars();
        }
    }
}
