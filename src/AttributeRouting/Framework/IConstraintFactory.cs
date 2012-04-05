using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AttributeRouting.Framework {
    public interface IConstraintFactory<out TConstraint> {

        TConstraint CreateRegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.None);

        TConstraint CreateRestfulHttpMethodConstraint(string[] httpMethods);
    }
}
