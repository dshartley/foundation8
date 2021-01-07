using System;
using System.Net;

namespace Smart.Platform.Net
{
    /// <summary>
    /// A helper class for network.
    /// </summary>
    public class NetHelper
    {
        #region Constructors

        private NetHelper() { }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Determines whether [is connected to internet].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is connected to internet]; otherwise, <c>false</c>.
        /// </returns>
        public static Exception IsConnectedToInternet()
        {
            #region Check Parameters

            #endregion

            try
            {
                using (var client = new WebClient())
                    using (client.OpenRead("http://google.com/generate_204"))
                        return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        #endregion
    }
}
