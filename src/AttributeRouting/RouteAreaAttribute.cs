using System;

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

        /// <summary>
        /// Key used by translation provider to lookup the translation for the <see cref="AreaUrl"/>.
        /// </summary>
        public string TranslationKey { get; set; }

        /// <summary>
        /// The subdomain that this area is mapped to. By default, areas apply to all subdomains.
        /// </summary>
        public string Subdomain { get; set; }
    }
}