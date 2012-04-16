using System;
using System.Collections.Generic;
using System.Linq;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;

namespace AttributeRouting.Logging
{
    public class AttributeRouteInfo
    {
        public AttributeRouteInfo()
        {
            Defaults = new Dictionary<string, string>();
            Constraints = new Dictionary<string, string>();
            DataTokens = new Dictionary<string, string>();
        }

        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public IDictionary<string, string> Defaults { get; set; }
        public IDictionary<string, string> Constraints { get; set; }
        public IDictionary<string, string> DataTokens { get; set; }

        public static AttributeRouteInfo GetRouteInfo(string url,
                                                      IDictionary<string, object> defaults,
                                                      IDictionary<string, object> constraints,
                                                      IDictionary<string, object> dataTokens)
        {

            var item = new AttributeRouteInfo { Url = url };

            if (defaults != null)
            {
                foreach (var @default in defaults)
                    item.Defaults.Add(@default.Key, @default.Value.ToString());
            }

            if (constraints != null)
            {
                foreach (var constraint in constraints)
                {
                    if (constraint.Value == null)
                        continue;

                    if (constraint.Value is IRestfulHttpMethodConstraint)
                        item.HttpMethod = String.Join(", ",
                                                      ((IRestfulHttpMethodConstraint)constraint.Value).AllowedMethods);
                    else if (constraint.Value is RegexRouteConstraintBase)
                        item.Constraints.Add(constraint.Key, ((RegexRouteConstraintBase)constraint.Value).Pattern);
                    else
                        item.Constraints.Add(constraint.Key, constraint.Value.ToString());
                }
            }

            if (dataTokens != null)
            {
                foreach (var token in dataTokens)
                {
                    if (token.Key.ValueEquals("namespaces"))
                        item.DataTokens.Add(token.Key, ((string[])token.Value).Aggregate((n1, n2) => n1 + ", " + n2));
                    else
                        item.DataTokens.Add(token.Key, token.Value.ToString());
                }
            }

            return item;
        }
    }
}