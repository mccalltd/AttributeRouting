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
        public string HttpMethods { get; set; }
        public IDictionary<string, string> Defaults { get; set; }
        public IDictionary<string, string> Constraints { get; set; }
        public IDictionary<string, string> DataTokens { get; set; }

        public static AttributeRouteInfo GetRouteInfo(string url,
                                                      IDictionary<string, object> defaults,
                                                      IDictionary<string, object> constraints,
                                                      IDictionary<string, object> dataTokens)
        {

            var item = new AttributeRouteInfo { Url = url };

            //************************
            // Defaults

            if (defaults != null)
            {
                foreach (var @default in defaults)
                {
                    var defaultValue = @default.Value.ToString();
                    item.Defaults.Add(@default.Key, defaultValue.ValueOr("Optional"));
                }
            }

            //************************
            // Constraints

            if (constraints != null)
            {
                foreach (var constraint in constraints)
                {
                    if (constraint.Value == null)
                        continue;

                    if (constraint.Value is IRestfulHttpMethodConstraint)
                    {
                        item.HttpMethods = String.Join(", ", ((IRestfulHttpMethodConstraint)constraint.Value).AllowedMethods);
                    }
                    else if (constraint.Value is RegexRouteConstraintBase)
                    {
                        item.Constraints.Add(constraint.Key, ((RegexRouteConstraintBase)constraint.Value).Pattern);
                    }
                    else
                    {
                        var constraintValue = constraint.Value;
                        var constraintDescriptions = new List<string>();

                        // Simple string regex constraint - from ASP.NET routing features
                        if (constraintValue is string)
                        {
                            constraintDescriptions.Add(constraintValue.ToString());
                        }
                        else
                        {
                            // Optional constraint - unwrap it and continue
                            var optionalConstraint = constraintValue as IOptionalRouteConstraintWrapper;
                            if (optionalConstraint != null)
                            {
                                constraintValue = optionalConstraint.Constraint;
                            }

                            // QueryString constraint - unwrap it and continue
                            var queryStringConstraint = constraintValue as IQueryStringRouteConstraintWrapper;
                            if (queryStringConstraint != null)
                            {
                                constraintValue = queryStringConstraint.Constraint;
                                constraintDescriptions.Add("QueryStringRouteConstraint");
                            }

                            // Compound constraint - join type names of the inner constraints
                            var compoundConstraint = constraintValue as ICompoundRouteConstraintWrapper;
                            if (compoundConstraint != null)
                            {
                                constraintDescriptions.AddRange(compoundConstraint.Constraints.Select(c => c.GetType().Name));
                            }
                            else if (constraintValue != null)
                            {
                                // Single constraint type
                                constraintDescriptions.Add(constraintValue.GetType().Name);
                            }                            
                        }

                        item.Constraints.Add(constraint.Key, String.Join(", ", constraintDescriptions));
                    }
                }
            }

            //************************
            // DataTokens

            if (dataTokens != null)
            {
                foreach (var token in dataTokens)
                {
                    if (token.Key.ValueEquals("namespaces"))
                    {
                        item.DataTokens.Add(token.Key, ((string[]) token.Value).Aggregate((n1, n2) => n1 + ", " + n2));
                    }
                    else if (!token.Key.ValueEquals("actionMethod"))
                    {
                        item.DataTokens.Add(token.Key, token.Value.ToString());
                    }
                }
            }

            return item;
        }
    }
}