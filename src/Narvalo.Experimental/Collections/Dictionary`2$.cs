namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using Narvalo;

    public static class DictionaryExtensions
    {
        public static TValue GetOrCreateValue<TKey, TValue>(
            this IDictionary<TKey, TValue> @this,
            TKey key,
            Func<TValue> createValueCallback)
        {
            Requires.Object(@this);

            if (!@this.ContainsKey(key)) {
                lock (@this) {
                    if (!@this.ContainsKey(key)) {
                        @this[key] = createValueCallback();
                    }
                }
            }

            return @this[key];
        }
    }
}