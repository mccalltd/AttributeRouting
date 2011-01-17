using System;
using System.Text.RegularExpressions;
using AttributeRouting.Extensions;

namespace AttributeRouting
{
    /// <summary>
    /// Defines an area to contain all the routes for this controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RouteAreaAttribute : Attribute
    {
        /// <summary>
        /// Specify an area to contain all the routes for this controller.
        /// </summary>
        /// <param name="areaName">The name of the containing area</param>
        public RouteAreaAttribute(string areaName)
        {
            if (areaName == null) throw new ArgumentNullException("areaName");
            if (Regex.IsMatch(areaName, @"\/") || !areaName.IsValidUrl())
                throw new ArgumentException(
                    ("The areaName \"{0}\" is not valid. It cannot contain forward slashes " +
                     "or any other character not allowed in URLs.").FormatWith(areaName));

            AreaName = areaName;
        }

        /// <summary>
        /// The area name that is registered for the routes in the controller.
        /// </summary>
        public string AreaName { get; private set; }

        /// <summary>
        /// The url prefix to apply to the routes.
        /// </summary>
        public string AreaUrl { get; set; }
    }
}