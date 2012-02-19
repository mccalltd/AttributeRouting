using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AttributeRouting.Framework;
using TechTalk.SpecFlow;

namespace AttributeRouting.Specs
{
    public static class Extensions
    {
        public static bool HasValue(this string s)
        {
            return !String.IsNullOrEmpty(s);
        }

        public static void RequireColumns(this Table table, params string[] requiredColumns)
        {
            var missingColumns = requiredColumns.Where(c => !table.Header.Contains(c));
            if (missingColumns.Any())
                throw new ApplicationException("The table is missing the following column definitions: " + String.Join(", ", missingColumns));    
        }

        public static AttributeRoute RequireRouteNamed(this IEnumerable<AttributeRoute> routes, string name)
        {
            var route = routes.FirstOrDefault(r => r.RouteName == name);
            if (route == null)
                throw new ApplicationException("There is no route named \"" + name + "\"");

            return route;
        }

        public static string TryGetValue(this TableRow row, string header)
        {
            if (row.Any(f => f.Key == header))
                return row[header];

            return null;
        }
    }
}
