using System;

namespace ApplicationProjectViews.AnalysisPageView
{
    public class AnalysisPageIncomeEntry
    {
        /// <summary>
        /// The displayed value of this entry
        /// </summary>
        public double Value { get; init; }

        /// <summary>
        /// The currency identifier to append to the value
        /// </summary>
        public string CurrencyIdentifier
        {
            get => m_CurrencyIdentifier;
            init => m_CurrencyIdentifier = value ?? throw new ArgumentNullException(nameof(CurrencyIdentifier));
        }
        private string m_CurrencyIdentifier;

        /// <summary>
        /// The text to display as the entry's title
        /// </summary>
        public string Title
        {
            get => m_Title;
            init => m_Title = value ?? throw new ArgumentNullException(nameof(Title));
        }
        private string m_Title;

        /// <summary>
        /// The category this entry belongs to
        /// </summary>
        public CategoryDescriptor Category
        {
            get => m_Category;
            init => m_Category = value ?? throw new ArgumentNullException(nameof(Category));
        }
        private CategoryDescriptor m_Category;

        /// <summary>
        /// The date of this entry
        /// </summary>
        public DateTime Date { get; init; }

        public AnalysisPageIncomeEntry()
        {
            Value = 0;
            CurrencyIdentifier = "";
            Title = "";
            Category = new CategoryDescriptor();
            Date = default;
        }
    }
}
