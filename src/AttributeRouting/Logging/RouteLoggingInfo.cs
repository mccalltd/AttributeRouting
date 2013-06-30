using System;
using System.Collections.Generic;
using System.Linq;
using AttributeRouting.Constraints;
using AttributeRouting.Helpers;

namespace AttributeRouting.Logging
{
    public class RouteLoggingInfo
    {
        public RouteLoggingInfo()
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

        public static RouteLoggingInfo GetRouteInfo(string url,
                                                      IDictionary<string, object> defaults,
                                                      IDictionary<string, object> queryStringDefaults,
                                                      IDictionary<string, object> constraints,
                                                      IDictionary<string, object> queryStringConstraints,
                                                      IDictionary<string, object> dataTokens)
        {

            var item = new RouteLoggingInfo { Url = url };

            //************************
            // Defaults

            var allDefaults = new Dictionary<string, object>();
            allDefaults.Merge(defaults);
            allDefaults.Merge(queryStringDefaults);
            
            foreach (var @default in allDefaults)
            {
                var defaultValue = @default.Value.ToString();
                item.Defaults.Add(@default.Key, defaultValue);
            }

            //************************
            // Constraints

            var allConstraints = new Dictionary<string, object>();
            allConstraints.Merge(constraints);
            allConstraints.Merge(queryStringConstraints);

            foreach (var constraint in allConstraints)
            {
                if (constraint.Value == null || constraint.Value is IInboundHttpMethodConstraint)
                    continue;

                var regexRouteConstraint = constraint.Value as RegexRouteConstraintBase;
                if (regexRouteConstraint != null)
                {
                    item.Constraints.Add(constraint.Key, regexRouteConstraint.Pattern);
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
                        var optionalConstraint = constraintValue as IOptionalRouteConstraint;
                        if (optionalConstraint != null)
                        {
                            constraintValue = optionalConstraint.Constraint;
                        }

                        // QueryString constraint - unwrap it and continue
                        var queryStringConstraint = constraintValue as IQueryStringRouteConstraint;
                        if (queryStringConstraint != null)
                        {
                            constraintValue = queryStringConstraint.Constraint;
                            constraintDescriptions.Add("QueryStringRouteConstraint");
                        }

                        // Compound constraint - join type names of the inner constraints
                        var compoundConstraint = constraintValue as ICompoundRouteConstraint;
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

            //************************
            // DataTokens

            if (dataTokens != null)
            {
                foreach (var token in dataTokens)
                {
                    if (token.Key.ValueEquals("namespaces"))
                    {
                        if (token.Value is string[])
                        {
                            item.DataTokens.Add(token.Key, String.Join(", ", (string[])token.Value));
                        }
                        else
                        {
                            item.DataTokens.Add(token.Key, String.Join(", ", (new string[] { token.Value.ToString() })));
                        }
                    }
                    else if (token.Key.ValueEquals("httpMethods"))
                    {
                        if (token.Value is string[])
                        {
                            item.HttpMethods = String.Join(", ", (string[])token.Value);
                        }
                        else 
                        {
                            item.HttpMethods = String.Join(", ", (new string[] { token.Value.ToString() }));
                        }
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