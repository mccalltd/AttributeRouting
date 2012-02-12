using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework.Localization
{
    public class ControllerTranslationBuilder<TController> where TController : IController
    {
        private readonly TranslationBuilder _builder;
        private readonly Type _controllerType;
        private readonly TranslationKeyGenerator _keyGenerator;

        public ControllerTranslationBuilder(TranslationBuilder builder)
        {
            _builder = builder;
            _controllerType = typeof(TController);
            _keyGenerator = new TranslationKeyGenerator();
        }

        public ControllerTranslationBuilder<TController> AreaUrl(Dictionary<string, string> cultureTranslationPairs)
        {
            var areaAttribute = _controllerType.GetCustomAttribute<RouteAreaAttribute>(true);

            if (areaAttribute == null)
                throw new AttributeRoutingException(
                    "There is no RouteAreaAttribute associated with {0}.".FormatWith(_controllerType.FullName));

            var key = _keyGenerator.AreaUrl(areaAttribute.AreaName);

            _builder.ForKey(key, cultureTranslationPairs);

            return this;
        }

        public ControllerTranslationBuilder<TController> RoutePrefix(Dictionary<string, string> cultureTranslationPairs)
        {
            var areaAttribute = _controllerType.GetCustomAttribute<RouteAreaAttribute>(true);

            var key = _keyGenerator.RoutePrefix(areaAttribute.SafeGet(a => a.AreaName),
                                                _controllerType.GetControllerName());

            _builder.ForKey(key, cultureTranslationPairs);

            return this;
        }

        public ControllerTranslationBuilder<TController> RouteUrl(Expression<Func<TController, object>> action, Dictionary<string, string> cultureTranslationPairs)
        {
            var areaAttribute = _controllerType.GetCustomAttribute<RouteAreaAttribute>(true);
            var actionMemberInfo = Helpers.ExpressionHelper.GetMethodInfo(action);

            var key = _keyGenerator.RouteUrl(areaAttribute.SafeGet(a => a.AreaName),
                                             _controllerType.GetControllerName(),
                                             actionMemberInfo.Name);

            _builder.ForKey(key, cultureTranslationPairs);

            return this;
        }
    }
}