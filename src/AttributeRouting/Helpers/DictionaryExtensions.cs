using System.Collections.Generic;

namespace AttributeRouting.Helpers
{
    public static class DictionaryExtensions
    {
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
        {
            if (source == null) return;

            foreach (var kvp in source)
            {
                target.Add(kvp.Key, kvp.Value);
            }
        }
    }
}
