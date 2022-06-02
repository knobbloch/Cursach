using System.Globalization;

namespace ApplicationProject.Views
{
    /// <summary>
    /// A descriptor of an input error for user input
    /// </summary>
    public class ValueInputError : ICultureDependentData
    {
        #region Nested Types
        /// <summary>
        /// All possible error states for a value input
        /// </summary>
        public enum ValueInputErrorType
        {
            /// <summary>
            /// No error
            /// </summary>
            None = 0,
            /// <summary>
            /// The value is considered empty
            /// </summary>
            EmptyValue = 1,
            /// <summary>
            /// The value is considered a duplicate
            /// </summary>
            DuplicateValue = 2,
            /// <summary>
            /// The value contains invalid symbols
            /// </summary>
            InvalidSymbol = 3,
            /// <summary>
            /// The value lies outside its expected bounds
            /// </summary>
            OutOfBoundsValue = 4
        }

        private readonly string[] LocaleKeys =
        {
            "",
            "VALUE_INPUT_ERROR_EMPTY_VALUE",
            "VALUE_INPUT_ERROR_DUPLICATE_VALUE",
            "VALUE_INPUT_ERROR_INVALID_SYMBOL",
            "VALUE_INPUT_ERROR_OUT_OF_BOUNDS"
        };
        #endregion

        public ValueInputError(ValueInputErrorType type, string info)
        {
            CurrentCulture = null;
            ErrorType = type;
            ErrorInfo = info;
        }

        #region ICultureDependentData
        protected CultureInfo CurrentCulture
        {
            get => m_CurrentCulture;
            set => m_CurrentCulture = value ?? System.Threading.Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture ?? CultureInfo.InvariantCulture;
        }
        private CultureInfo m_CurrentCulture;

        public void OnCultureChanged(CultureInfo newCulture)
        {
            CurrentCulture = newCulture;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Defines the type of the error
        /// </summary>
        public ValueInputErrorType ErrorType { get; }

        /// <summary>
        /// Contains extra information regarding the error. The value dependes on the ErrorType:
        /// None, EmptyValue, DuplicateValue - the value is unused
        /// InvalidSymbol - the symbol (or sequence of symbols) which are considered invalid of the given input
        /// OutOfBoundsValue - the expected bounds for the value
        /// </summary>
        public string ErrorInfo { get; }
        #endregion

        public override string ToString()
        {
            return Resources.Locale.ResourceManager.GetString(LocaleKeys[(int)ErrorType], CurrentCulture);
        }
    }
}
