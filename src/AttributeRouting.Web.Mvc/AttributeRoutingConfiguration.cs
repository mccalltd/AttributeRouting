using System.Web.Mvc;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Framework.Factories;
using AttributeRouting.Web.Mvc.Framework.Factories;

namespace AttributeRouting.Web.Mvc
{
    public class AttributeRoutingConfiguration : WebAttributeRoutingConfiguration<IController, UrlParameter>
    {
        private readonly IAttributeRouteFactory _attributeFactory;
        private readonly IParameterFactory<UrlParameter> _parameterFactory;

        public AttributeRoutingConfiguration()
            : base(() => new MvcRouteHandler()) {
            _attributeFactory = new MvcAttributeRouteFactory(this);
            _parameterFactory = new UrlParameterFactory();
        }

        /// <summary>
        /// Attribute factory
        /// </summary>
        public override IAttributeRouteFactory AttributeFactory {
            get { return _attributeFactory; }
        }

        /// <summary>
        /// Parameter factory
        /// </summary>
        public override IParameterFactory<UrlParameter> ParameterFactory {
            get { return _parameterFactory; }
        }
    }
}
