using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace AttributeRouting.Framework.Localization
{
    public class ControllerTranslationBuilder<TController> where TController : IController
    {
        private readonly TranslationBuilder _builder;
        private readonly TranslationKeyGenerator _keyGenerator;

        public ControllerTranslationBuilder(TranslationBuilder builder)
        {
            _builder = builder;
            _keyGenerator = new TranslationKeyGenerator();
        }

        public ControllerTranslationBuilder<TController> AreaUrl(Dictionary<string, string> cultureTranslationPairs)
        {
            var key = _keyGenerator.AreaUrl<TController>();
            _builder.ForKey(key, cultureTranslationPairs);

            return this;
        }

        public ControllerTranslationBuilder<TController> RoutePrefix(Dictionary<string, string> cultureTranslationPairs)
        {
            var key = _keyGenerator.RoutePrefixUrl<TController>();
            _builder.ForKey(key, cultureTranslationPairs);

            return this;
        }

        public ControllerTranslationBuilder<TController> RouteUrl(Expression<Func<TController, object>> action, Dictionary<string, string> cultureTranslationPairs)
        {
            var key = _keyGenerator.RouteUrl(action);
            _builder.ForKey(key, cultureTranslationPairs);

            return this;
        }
    }
}