using System;
using System.Collections;

namespace Smart.Platform.Data
{
    /// <summary>
    /// A base class for classes which compare IDataItem objects
    /// </summary>
    class DataItemComparerBase : IComparer
    {
        private int _propertyEnumToCompare;

        #region Constructors

        private DataItemComparerBase() { }

        public DataItemComparerBase(int propertyEnumToCompare)
        {
            _propertyEnumToCompare = propertyEnumToCompare;
        }

        #endregion

        #region IComparer Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value
        /// Condition
        /// Less than zero
        /// <paramref name="x"/> is less than <paramref name="y"/>.
        /// Zero
        /// <paramref name="x"/> equals <paramref name="y"/>.
        /// Greater than zero
        /// <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// Neither <paramref name="x"/> nor <paramref name="y"/> implements the <see cref="T:System.IComparable"/> interface.
        /// -or-
        /// <paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other.
        /// </exception>
        public int Compare(object x, object y)
        {
            // Initially represent an undetermined comparison
            int result = -1;

            string x_val = ((IDataItem)x).GetProperty(_propertyEnumToCompare);
            if (x_val == null) x_val = "";
            string y_val = ((IDataItem)y).GetProperty(_propertyEnumToCompare);
            if (y_val == null) y_val = "";
            
            // Number
            decimal x_dec;
            decimal y_dec;
            if (decimal.TryParse(x_val, out x_dec) && decimal.TryParse(y_val, out y_dec))
            {
                result = decimal.Compare(x_dec, y_dec);
                return result;
            }

            // Date
            DateTime x_date;
            DateTime y_date;
            if (DateTime.TryParse(x_val, out x_date) && DateTime.TryParse(y_val, out y_date))
            {
                result = x_date.CompareTo(y_date);
                return result;
            }

            // String
            result = string.Compare(x_val, y_val);
            return result;
        }

        #endregion
    }
}
