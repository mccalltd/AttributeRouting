namespace AttributeRouting.Framework.Localization
{
    public interface ITranslationProvider
    {
        string Translate(string key, string culture);
    }
}