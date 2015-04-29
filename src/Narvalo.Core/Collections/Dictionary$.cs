// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="IDictionary{T,U}"/> that depend on the <see cref="Maybe{T}"/> class.
    /// </summary>
    public static partial class DictionaryExtensions
    {
        /// <summary>
        /// Version sans passage par référence de la méthode <see cref="System.Collections.Generic.IDictionary{T,U}.TryGetValue"/>.
        /// </summary>
        /// <typeparam name="TKey">Le type des clés du dictionnaire.</typeparam>
        /// <typeparam name="TValue">Le type des valeurs du dictionnaire.</typeparam>
        /// <param name="this">Le dictionnaire à analyser.</param>
        /// <param name="key">La clé à rechercher.</param>
        /// <returns>Retourne une monade Maybe contenant la valeur associée à la clé.</returns>
        public static Maybe<TValue> MayGetValue<TKey, TValue>(
            this IDictionary<TKey, TValue> @this,
            TKey key)
        {
            Require.Object(@this);

            if (key == null) { return Maybe<TValue>.None; }

            TValue value;
            return @this.TryGetValue(key, out value) ? Maybe.Of(value) : Maybe<TValue>.None;
        }
    }
}
