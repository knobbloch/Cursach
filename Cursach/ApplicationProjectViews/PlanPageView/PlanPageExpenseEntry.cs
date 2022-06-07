using System;

namespace ApplicationProjectViews.PlanPageView
{
    public class PlanPageExpenseEntry
    {
        /// <summary>
        /// The displayed real value of this entry
        /// </summary>
        public double RealValue { get; init; }

        /// <summary>
        /// The displayed planned value of this entry
        /// </summary>
        public double PlannedValue { get; init; }

        /// <summary>
        /// The difference between real and planned values
        /// </summary>
        public double Difference => RealValue - PlannedValue;

        /// <summary>
        /// The currency identifier to append to the values
        /// </summary>
        public string CurrencyIdentifier
        {
            get => m_CurrenyIdentifier;
            init => m_CurrenyIdentifier = value ?? throw new ArgumentNullException(nameof(CurrencyIdentifier));
        }
        private string m_CurrenyIdentifier;

        /// <summary>
        /// The text to display as the chart entry's title
        /// </summary>
        public string Title
        {
            get => m_Title;
            init => m_Title = value ?? throw new ArgumentNullException(nameof(CurrencyIdentifier));
        }
        private string m_Title;

        public string ImagePath
        {
            get => m_ImagePath;
            set => m_ImagePath = value ?? throw new ArgumentNullException(nameof(ImagePath));
        }
        private string m_ImagePath;

        public PlanPageExpenseEntry()
        {
            RealValue = 0;
            PlannedValue = 0;
            Title = "";
            CurrencyIdentifier = "";
            ImagePath = "";
        }
    }
}
