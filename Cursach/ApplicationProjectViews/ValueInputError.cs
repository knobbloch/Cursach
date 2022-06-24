namespace ApplicationProjectViews
{
    /// <summary>
    /// A descriptor of an input error for user input
    /// </summary>
    public class ValueInputError
    {
        #region Nested Types
        /// <summary>
        /// All possible error states for a value input
        /// </summary>
        public enum ValueInputErrorType
        {
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
        #endregion

        public ValueInputError(ValueInputErrorType type, string info)
        {
            ErrorType = type;
            ErrorInfo = info;
        }

        #region Properties
        /// <summary>
        /// Defines the type of the error
        /// </summary>
        public ValueInputErrorType ErrorType { get; init; }

        /// <summary>
        /// Contains extra information regarding the error. The value dependes on the ErrorType:
        /// None, EmptyValue, DuplicateValue - the value is unused
        /// InvalidSymbol - the symbol (or sequence of symbols) which are considered invalid of the given input
        /// OutOfBoundsValue - the expected bounds for the value
        /// </summary>
        public string ErrorInfo { get; init; }
        #endregion
    }
}
