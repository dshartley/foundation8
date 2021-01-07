using System;
using System.Collections;
using System.Xml;
using Smart.Platform.Diagnostics;
using Smart.Platform.Globalization.Countries.Resources;

namespace Smart.Platform.Globalization.Countries
{
    /// <summary>
    /// Manages a localizable set of countries
    /// </summary>
    public class Countries
    {
        private static XmlDocument  _xmlDocument    = LoadXml();
        private static ArrayList    _items          = FillItems();

        #region Constructors

        private Countries() { }

        #endregion

        #region Public Static Methods

        public static XmlDocument XmlDocument
        {
            get
            {
                return _xmlDocument;
            }
        }

        public static ArrayList Items
        {
            get
            {
                return _items;
            }
        }

        public static Country GetCountryByCode(string code)
        {
            #region Check Parameters

            if (code == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "code is nothing"));

            #endregion

            // Go through all items in the list
            foreach (Country country in _items)
            {
                if (country.Code == code) return country;
            }
            return null;
        }

        public static Country GetCountryByName(string name)
        {
            #region Check Parameters

            if (name == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "name is nothing"));

            #endregion

            // Go through all items in the list
            foreach (Country country in _items)
            {
                if (country.Name == name) return country;
            }
            return null;
        }

        #endregion

        #region Private Static Methods

        private static XmlDocument LoadXml()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(CountriesData.Xml);

            return document;
        }

        private static ArrayList FillItems()
        {
            ArrayList items = new ArrayList();

            // Fill the list
            foreach (XmlNode node in _xmlDocument.DocumentElement.ChildNodes)
            {
                string countryName = node.InnerText;
                string countryCode = node.Attributes["code"].Value;

                // Add it to the list
                items.Add(new Country(countryCode, countryName));
            }  

            return items;
        }

        #endregion

    }
}
