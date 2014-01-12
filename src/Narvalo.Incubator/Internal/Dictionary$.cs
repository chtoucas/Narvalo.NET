namespace Narvalo.Internal
{
    using System;
    using System.Collections.Generic;
    using Narvalo;

    static class DictionaryExtensions
    {
        public static TValue GetOrCreateValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key,
            Func<TValue> createValueCallback)
        {
            Requires.Object(dict);

            if (!dict.ContainsKey(key)) {
                lock (dict) {
                    if (!dict.ContainsKey(key)) {
                        dict[key] = createValueCallback();
                    }
                }
            }

            return dict[key];
        }
    }
}