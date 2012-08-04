using System.Web.Routing;
using AttributeRouting.Constraints;
using AttributeRouting.Web.Framework.Factories;
using AttributeRouting.Web.Mvc.Constraints;

namespace AttributeRouting.Web.Mvc.Framework.Factories
{
    internal class RouteConstraintFactory : WebRouteConstraintFactoryBase
    {
        public RouteConstraintFactory(AttributeRoutingConfiguration configuration) 
            : base(configuration)
        { }

        public override IOptionalRouteConstraintWrapper CreateOptionalRouteConstraint(object constraint)
        {
            return new OptionalRouteConstraintWrapper((IRouteConstraint)constraint);
        }
    }
}