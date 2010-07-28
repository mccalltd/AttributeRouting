using System.Collections.Generic;

namespace AttributeRouting
{
    internal class AttributeRouteSpecificationComparer : EqualityComparer<AttributeRouteSpecification>
    {
        public override bool Equals(AttributeRouteSpecification x, AttributeRouteSpecification y)
        {
            return x.Url == y.Url &&
                   x.HttpMethod == y.HttpMethod &&
                   x.ControllerType == y.ControllerType &&
                   x.ActionName == y.ActionName;
        }

        public override int GetHashCode(AttributeRouteSpecification obj)
        {
            if (obj == null)
                return 0;

            var hashObj = "{0}_{1}_{2}_{3}".FormatWith(obj.Url,
                                                       obj.HttpMethod,
                                                       obj.ControllerType.FullName,
                                                       obj.ActionName);
            
            return hashObj.GetHashCode();
        }
    }
}