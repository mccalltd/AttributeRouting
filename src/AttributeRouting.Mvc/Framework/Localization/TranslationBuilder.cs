using System.Web.Mvc;

namespace AttributeRouting.Mvc.Framework.Localization {
    public class TranslationBuilder {

        /// <summary>
        /// Returns a <see cref="AttributeRouting.Mvc.Framework.Localization.ControllerTranslationBuilder{TController}"/>
        /// for adding translations of route components in a strongly typed manner.
        /// </summary>
        /// <typeparam name="TController">The type of the controller for which to add translations</typeparam>
        public ControllerTranslationBuilder<TController> ForController<TController>()
            where TController : IController {
            return new ControllerTranslationBuilder<TController>(this);
        }
    }
}
