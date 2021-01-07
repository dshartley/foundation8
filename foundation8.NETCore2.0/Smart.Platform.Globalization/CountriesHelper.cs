using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Smart.Platform.Globalization
{
    /// <summary>
    /// Provides helper methods for countries data.
    /// </summary>
    public class CountriesHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesHelper"/> class.
        /// </summary>
        private CountriesHelper() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a sorted list of countries.
        /// </summary>
        /// <returns></returns>
        public static SortedDictionary<string, string> GetCountries() 
        {
            // Create a sorted collection for the countries
            SortedDictionary<string, string> d = new SortedDictionary<string, string>();

            // Go through each culture info
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))     
            {
                // Get the region info for the culture info identifier
                RegionInfo ri = new RegionInfo(ci.LCID);

                // This alphabetic comparison is done to omit any non-english language and unreadable countries
                if ((String.Compare(ri.DisplayName.ToLower(), "a") > 0) && (String.Compare(ri.DisplayName.ToLower(), "zz") < 0))
                {
                    // If it's not been added then add the country to the collection
                    if (!d.ContainsKey(ri.DisplayName) && !d.ContainsValue(ri.TwoLetterISORegionName))
                    {
                        d.Add(ri.DisplayName, ri.TwoLetterISORegionName); // The collection is sorted on the 'key'   
                    }
                }
            }

            return d;
        }

        #endregion
    }
}
