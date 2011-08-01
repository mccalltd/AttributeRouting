using System;

namespace AttributeRouting
{
    /// <summary>
    /// Defines a prefix to be used before all routes defined in this controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RoutePrefixAttribute : Attribute
    {
        /// <summary>
        /// Specify a prefix to be used before all routes defined in this controller.
        /// </summary>
        /// <param name="url">The url prefix to apply to the routes</param>
        public RoutePrefixAttribute(string url)
        {
            if (url == null) throw new ArgumentNullException("url");

            Url = url;
        }

        /// <summary>
        /// The url prefix to apply to the routes.
        /// </summary>
        public string Url { get; private set; }
    }
}