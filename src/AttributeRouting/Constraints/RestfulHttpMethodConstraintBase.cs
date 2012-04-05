using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeRouting.Constraints
{
    public abstract class RestfulHttpMethodConstraintBase : IRestfulHttpMethodConstraint
    {
        /// <summary>
        /// Constrain a route by the specified allowed HTTP methods.
        /// </summary>
        protected RestfulHttpMethodConstraintBase(params string[] allowedMethods)
        {
            AllowedMethods = new List<string>(allowedMethods);
        }

        protected bool IsMatch(bool urlGeneration, string httpMethod)
        {
            if (urlGeneration)
                return true;

            return AllowedMethods.Any(m => m.Equals(httpMethod, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Allowed HTTP methods for RESTful route
        /// </summary>
        public ICollection<string> AllowedMethods { get; private set; }
    }
}
