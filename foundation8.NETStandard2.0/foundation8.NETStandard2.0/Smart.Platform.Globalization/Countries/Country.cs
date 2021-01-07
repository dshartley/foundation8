using System;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Globalization.Countries
{
    /// <summary>
    /// Encapsulates a country.
    /// </summary>
    public class Country
    {
        #region Constructors

        private Country() { }

        public Country(string code, string name)
        {
            #region Check Parameters

            if (code == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "code is nothing"));
            if (name == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "name is nothing"));

            #endregion

            _code = code;
            _name = name;
        }

        #endregion

        #region Public Methods

        private string _code;

        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }

        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        #endregion
    }
}
