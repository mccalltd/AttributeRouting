using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AttributeRouting
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RoutePrefixAttribute : Attribute
    {
        public RoutePrefixAttribute(string url)
        {
            if (url == null) throw new ArgumentNullException("url");
            if (Regex.IsMatch(url, @"^\/|\/$") || !url.IsValidUrl(true))
                throw new ArgumentException(
                    ("The url \"{0}\" is not valid. It cannot start or end with forward slashes " +
                     "or contain any other character not allowed in URLs.").FormatWith(url), "url");

            Url = url;
        }

        public string Url { get; private set; }
    }
}
