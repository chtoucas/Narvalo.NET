// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Samples
{
    using System;
    using Narvalo.Edu.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="Narvalo.Edu.Samples.Identity{T}"/>.
    /// </summary>
    static partial class IdentityExtensions
    {
        #region Monadic lifting operators

        public static Identity<TResult> Zip<TFirst, TSecond, TResult>(
            this Identity<TFirst> @this,
            Identity<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            return @this.Bind(_ => second.Map(middle => resultSelector.Invoke(_, middle)));
        }

        #endregion

        #region Additional methods

        public static Identity<TResult> Coalesce<TSource, TResult>(
            this Identity<TSource> @this,
            Func<TSource, bool> predicate,
            Identity<TResult> then,
            Identity<TResult> otherwise)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return predicate.Invoke(@this.Value) ? then : otherwise;
        }

        public static Identity<Unit> Run<TSource>(this Identity<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            action.Invoke(@this.Value); 

            return Identity.Unit;
        }

        #endregion
    }
}
