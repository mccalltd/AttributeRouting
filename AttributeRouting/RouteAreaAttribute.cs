using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AttributeRouting
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RouteAreaAttribute : Attribute
    {
        public RouteAreaAttribute(string areaName)
        {
            if (areaName == null) throw new ArgumentNullException("areaName");
            if (Regex.IsMatch(areaName, @"\/") || !areaName.IsValidUrl())
                throw new ArgumentException(
                    ("The areaName \"{0}\" is not valid. It cannot contain forward slashes " +
                     "or any other character not allowed in URLs.").FormatWith(areaName), "areaName");
            
            AreaName = areaName;
        }

        public string AreaName { get; private set; }

        public string AreaUrl { get; set; }
    }
}
