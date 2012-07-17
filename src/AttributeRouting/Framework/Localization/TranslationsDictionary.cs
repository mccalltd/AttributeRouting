using System.Collections.Generic;

namespace AttributeRouting.Framework.Localization
{
    /// <summary>
    /// Dictionary of keys and dictionaries of culture name and translation pairs.
    /// <code>
    /// var translations = new TranslationsDictionary();
    /// ...
    /// var translation = translations[key][cultureName];
    /// </code>
    /// </summary>
    public class TranslationsDictionary : Dictionary<string, IDictionary<string, string>> { }
}