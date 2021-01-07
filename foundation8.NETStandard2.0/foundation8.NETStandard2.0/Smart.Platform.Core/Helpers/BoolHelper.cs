namespace Smart.Platform.Core
{
    /// <summary>
    /// Provides helper methods for bool.
    /// </summary>
    public class BoolHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolHelper"/> class.
        /// </summary>
        private BoolHelper() { }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// To the bool.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool ToBool(string value)
        {
            #region Check Parameters

            #endregion

            bool result = false;

            result = (!string.IsNullOrEmpty(value) 
                && (value.ToLower() == "true" || value == "1")) ? true : false;

            return result;
        }

        #endregion

        #region Private Static Methods

        #endregion
    }
}
