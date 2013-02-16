using System;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Strategies for generating conventional route names.
    /// </summary>
    /// <remarks>
    /// A bit awkward currently due to initial impl not being formalized into separate strategy classes.
    /// Will refactor use via configuration API for v4.0.
    /// </remarks>
    public static class RouteNameBuilders
    {
        /// <summary>
        /// This builder ensures that every route has a unique name.
        /// Preferably, it generates routes names like "Area_Controller_Action".
        /// In case of duplicates, it will append the HTTP method (if not a GET route) and/or a unique index to the route.
        /// So the most heinous possible form is "Area_Controller_Action_Method_Index".
        /// </summary>
        public static Func<RouteSpecification, string> Unique
        {
            get { return new UniqueRouteNameBuilder().Execute; }
        }

        /// <summary>
        /// This builder generates routes in the form "Area_Controller_Action". 
        /// In case of duplicates, the duplicate route is not named, and the builder will return null.
        /// </summary>
        public static Func<RouteSpecification, string> FirstInWins
        {
            get { return new FirstInWinsRouteNameBuilder().Execute; }
        }
    }
}