using System.ComponentModel;

namespace ApplicationProject.Views
{
    /// <summary>
    /// A descriptor of an input error for user input
    /// </summary>
    public class ValueInputError : INotifyPropertyChanged
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
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Defines the type of the error
        /// </summary>
        public ValueInputErrorType ErrorType
        {
            get => m_ErrorType;
            set
            {
                m_ErrorType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorType)));
            }
        }
        private ValueInputErrorType m_ErrorType;

        /// <summary>
        /// Contains extra information regarding the error. The value dependes on the ErrorType:
        /// None, EmptyValue, DuplicateValue - the value is unused
        /// InvalidSymbol - the symbol (or sequence of symbols) which are considered invalid of the given input
        /// OutOfBoundsValue - the expected bounds for the value
        /// </summary>
        public string ErrorInfo
        {
            get => m_ErrorInfo;
            set
            {
                m_ErrorInfo = value ?? "";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorInfo)));
            }
        }
        private string m_ErrorInfo;
        #endregion
    }
}
