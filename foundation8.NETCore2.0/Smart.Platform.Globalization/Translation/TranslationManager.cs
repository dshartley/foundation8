using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Threading;

namespace Smart.Platform.Globalization.Translation
{
    /// <summary>
    /// Manages translation of localizable elements
    /// </summary>
    class TranslationManager
    {

        public String GetFormattedValue()
        {
            // Get the culture
            CultureInfo ci = new CultureInfo("fr-FR");
            int i = 100;

            // Return the currency
            return i.ToString("C", ci);
        }

    }
}
