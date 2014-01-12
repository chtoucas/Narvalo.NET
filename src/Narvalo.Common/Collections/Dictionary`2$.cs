namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using Narvalo.Fx;

    public static class DictionaryExtensions
    {
        public static Maybe<TValue> MayGetValue<TKey, TValue>(
            this IDictionary<TKey, TValue> @this,
            TKey key)
        {
            Requires.Object(@this);

            if (key == null) { return Maybe<TValue>.None; }

            TValue value;
            return @this.TryGetValue(key, out value) ? Maybe.Create(value) : Maybe<TValue>.None;
        }
    }
}
