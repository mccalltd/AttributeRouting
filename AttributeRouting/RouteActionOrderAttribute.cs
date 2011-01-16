using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeRouting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RouteActionOrderAttribute : Attribute
    {
        /// <summary>
        /// Specify the order of the routes defined for this action within the context of the declaring controller.
        /// </summary>
        /// <param name="actionOrder">The order of this action's routes within the controller</param>
        public RouteActionOrderAttribute(int actionOrder)
        {
            ActionOrder = actionOrder;
        }

        public int ActionOrder { get; set; }
    }
}
