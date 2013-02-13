using System;
using System.Collections.Generic;
using System.ComponentModel;
using AttributeRouting.Helpers;

namespace AttributeRouting.Constraints
{
    /// <summary>
    /// Constrains a url parameter by the values in the given enum type.
    /// </summary>
    public abstract class EnumValueRouteConstraintBase<T> : IAttributeRouteConstraint 
        where T : struct
    {
        private readonly TypeConverter _converter;

        protected EnumValueRouteConstraintBase()
        {
            var valueType = Enum.GetUnderlyingType(typeof(T));
            _converter = TypeDescriptor.GetConverter(valueType);
        }

        public bool IsMatch(string parameterName, IDictionary<string, object> routeValues)
        {
            var value = routeValues[parameterName];
            if (value.HasNoValue())
                return true;

            if (!_converter.IsValid(value))
                return false;

            return Enum.IsDefined(typeof(T), _converter.ConvertFrom(value));
        }
    }
}