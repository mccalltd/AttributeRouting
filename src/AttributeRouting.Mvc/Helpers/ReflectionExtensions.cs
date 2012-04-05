using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace AttributeRouting.Mvc.Helpers {
    public static class ReflectionExtensions {
        public static IEnumerable<Type> GetControllerTypes(this Assembly assembly) {
            return from type in assembly.GetTypes()
                   where !type.IsAbstract && typeof (IController).IsAssignableFrom(type)
                   select type;
        }
    }
}
