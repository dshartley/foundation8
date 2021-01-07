using System.Collections.Generic;
using System.Globalization;

namespace Smart.Platform.Globalization.Translation
{
    /// <summary>
    /// Defines a class which encapsulates a set of translatable words.
    /// </summary>
    interface ITranslationWordSet
    {
        // Returns a dictionary of values for the specified culture
        Dictionary<int, string> GetTranslation(CultureInfo cultureInfo);
    }
}
