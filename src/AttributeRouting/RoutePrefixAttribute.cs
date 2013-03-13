using System;

namespace AttributeRouting
{
    /// <summary>
    /// Defines a prefix shared by all the routes defined in this controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RoutePrefixAttribute : Attribute
    {
        /// <summary>
        /// Defines a prefix shared by all the routes defined in this controller.
        /// The url prefix will be the name of the controller without the "Controller" suffix.
        /// </summary>
        public RoutePrefixAttribute()
        {
            Precedence = int.MaxValue;
        }

        /// <summary>
        /// Defines a prefix shared by all the routes defined in this controller.
        /// </summary>
        /// <param name="url">The url prefix to apply to the routes</param>
        public RoutePrefixAttribute(string url)
            : this()
        {
            if (url == null) throw new ArgumentNullException("url");

            Url = url;
        }

        /// <summary>
        /// The url prefix to apply to the routes.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// The order of the routes using the given prefix 
        /// among all the routes generated with prefixes for this controller.
        /// </summary>
        /// <remarks>
        /// Positive integers (including zero) denote top routes: 1 is first, 2 is second, etc....
        /// Negative integers denote bottom routes: -1 is last, -2 is second to last, etc....
        /// </remarks>
        public int Precedence { get; set; }
    }
}