using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AttributeRouting.Framework {
    public interface IConstraintFactory {

        TConstraint CreateRegexRouteConstraint<TConstraint>(string pattern, RegexOptions options = RegexOptions.None);

        TConstraint CreateRestfulHttpMethodConstraint<TConstraint>(string[] httpMethods);
    }
}
